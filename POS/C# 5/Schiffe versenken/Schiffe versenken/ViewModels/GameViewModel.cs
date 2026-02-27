using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Schiffe_versenken.Controller;
using Schiffe_versenken.Models;
using Schiffe_versenken.Views;

namespace Schiffe_versenken.ViewModels;

public class GameViewModel : BaseViewModel
{
    private readonly BoardController _ownBoardController;
    private readonly BoardController _enemyBoardController;
    private NetworkController? _networkController;

    private GamePhase _gamePhase = GamePhase.WaitingForConnection;
    private bool _isMyTurn;
    private bool _isPlacingHorizontal = true;
    private Ship? _selectedShip;
    private string _statusMessage = "Warte auf Verbindung...";
    private string _connectionAddress = "127.0.0.1";
    private int _connectionPort = 5000;
    private bool _isHost;
    private bool _opponentReady;
    private int _lastAttackX, _lastAttackY;

    public GameViewModel()
    {
        _ownBoardController = new BoardController();
        _enemyBoardController = new BoardController();

        ShipsToPlace = new ObservableCollection<Ship>(BoardController.CreateStandardShips());
        OwnBoardCells = [];
        EnemyBoardCells = [];

        InitializeBoardCells();

        // Board ViewModels
        OwnBoardViewModel = new BoardViewViewModel(OwnBoardCells, "Eigenes Spielfeld");
        EnemyBoardViewModel = new BoardViewViewModel(EnemyBoardCells, "Gegnerisches Spielfeld");

        // Erstes Schiff auswählen
        SelectedShip = ShipsToPlace.FirstOrDefault();

        // Commands
        StartAsHostCommand = new RelayCommand(StartAsHost, _ => GamePhase == GamePhase.WaitingForConnection);
        ConnectToHostCommand = new RelayCommand(ConnectToHost, _ => GamePhase == GamePhase.WaitingForConnection);
        RotateShipCommand = new RelayCommand(_ => RotateShip(), _ => GamePhase == GamePhase.PlacingShips);
        StartGameCommand = new RelayCommand(_ => ConfirmShipsPlaced(), _ => CanStartGame());
        ResetBoardCommand = new RelayCommand(_ => ResetBoard(), _ => GamePhase == GamePhase.PlacingShips);
        SelectShipCommand = new RelayCommand(SelectShip, _ => GamePhase == GamePhase.PlacingShips);
    }

    #region Properties

    public ObservableCollection<Ship> ShipsToPlace { get; }
    public ObservableCollection<CellViewModel> OwnBoardCells { get; }
    public ObservableCollection<CellViewModel> EnemyBoardCells { get; }
    
    public BoardViewViewModel OwnBoardViewModel { get; }
    public BoardViewViewModel EnemyBoardViewModel { get; }

    public GamePhase GamePhase
    {
        get => _gamePhase;
        set
        {
            if (SetProperty(ref _gamePhase, value))
            {
                UpdateStatusMessage();
                OnPropertyChanged(nameof(IsSetupPhase));
                OnPropertyChanged(nameof(IsConnectionPhase));
                CommandManager.InvalidateRequerySuggested();
            }
        }
    }

    public bool IsMyTurn
    {
        get => _isMyTurn;
        set
        {
            if (SetProperty(ref _isMyTurn, value))
            {
                UpdateStatusMessage();
            }
        }
    }

    public bool IsPlacingHorizontal
    {
        get => _isPlacingHorizontal;
        set => SetProperty(ref _isPlacingHorizontal, value);
    }

    public Ship? SelectedShip
    {
        get => _selectedShip;
        set => SetProperty(ref _selectedShip, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public string ConnectionAddress
    {
        get => _connectionAddress;
        set => SetProperty(ref _connectionAddress, value);
    }

    public int ConnectionPort
    {
        get => _connectionPort;
        set => SetProperty(ref _connectionPort, value);
    }

    public bool IsHost
    {
        get => _isHost;
        private set => SetProperty(ref _isHost, value);
    }

    public bool IsSetupPhase => GamePhase == GamePhase.PlacingShips;
    public bool IsConnectionPhase => GamePhase == GamePhase.WaitingForConnection;

    #endregion

    #region Commands

    public ICommand StartAsHostCommand { get; }
    public ICommand ConnectToHostCommand { get; }
    public ICommand RotateShipCommand { get; }
    public ICommand StartGameCommand { get; }
    public ICommand ResetBoardCommand { get; }
    public ICommand SelectShipCommand { get; }

    #endregion

    #region Methods

    private void InitializeBoardCells()
    {
        OwnBoardCells.Clear();
        EnemyBoardCells.Clear();

        for (int y = 0; y < Board.BoardSize; y++)
        {
            for (int x = 0; x < Board.BoardSize; x++)
            {
                OwnBoardCells.Add(new CellViewModel(_ownBoardController.Board.Fields[x, y], x, y, false, this));
                EnemyBoardCells.Add(new CellViewModel(_enemyBoardController.Board.Fields[x, y], x, y, true, this));
            }
        }
    }

    private void RegisterNetworkEvents()
    {
        if (_networkController == null) return;
        _networkController.OnReadyReceived += HandleReadyReceived;
        _networkController.OnStartPlayerReceived += HandleStartPlayerReceived;
        _networkController.OnTurnReceived += HandleTurnReceived;
        _networkController.OnAnswerReceived += HandleAnswerReceived;
        _networkController.OnConnectionLost += HandleConnectionLost;
    }

    private async void StartAsHost(object? parameter)
    {
        try
        {
            IsHost = true;
            _networkController = new NetworkController();
            RegisterNetworkEvents();

            StatusMessage = $"Warte auf Verbindung auf Port {ConnectionPort}...";
            await _networkController.StartHostAsync(ConnectionPort);

            GamePhase = GamePhase.PlacingShips;
            StatusMessage = "Gegner verbunden! Platziere deine Schiffe.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Fehler beim Starten des Servers: {ex.Message}";
        }
    }

    private async void ConnectToHost(object? parameter)
    {
        try
        {
            IsHost = false;
            _networkController = new NetworkController();
            RegisterNetworkEvents();

            StatusMessage = $"Verbinde mit {ConnectionAddress}:{ConnectionPort}...";
            await _networkController.ConnectToHostAsync(ConnectionAddress, ConnectionPort);

            GamePhase = GamePhase.PlacingShips;
            StatusMessage = "Verbunden. Platziere deine Schiffe.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Verbindungsfehler: {ex.Message}";
        }
    }

    private void HandleReadyReceived()
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            _opponentReady = true;
            if (GamePhase == GamePhase.WaitingForOpponent && IsHost)
            {
                BeginGame();
            }
        });
    }

    private void HandleStartPlayerReceived(StartPlayerPacket packet)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            IsMyTurn = packet.IsFirstPlayer;
            GamePhase = GamePhase.Playing;
        });
    }

    private void HandleTurnReceived(TurnPacket packet)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            HandleIncomingAttack(packet.X, packet.Y);
        });
    }

    private void HandleAnswerReceived(AnswerPacket packet)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            HandleAttackResult(packet.Hit, packet.Sunk, packet.GameOver);
        });
    }

    private void HandleConnectionLost()
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            StatusMessage = "Verbindung verloren";
            GamePhase = GamePhase.GameOver;
        });
    }

    private void SelectShip(object? parameter)
    {
        if (parameter is Ship ship && !ship.IsPlaced)
        {
            SelectedShip = ship;
            StatusMessage = $"Platziere: {ship.Name} (Größe: {ship.Size})";
        }
    }

    public void PlaceShipAtCell(int x, int y)
    {
        if (GamePhase != GamePhase.PlacingShips || SelectedShip == null)
            return;

        if (_ownBoardController.PlaceShip(SelectedShip, x, y, IsPlacingHorizontal))
        {
            ShipsToPlace.Remove(SelectedShip);
            SelectedShip = ShipsToPlace.FirstOrDefault();
            RefreshOwnBoardCells();

            if (ShipsToPlace.Count == 0)
            {
                StatusMessage = "Alle Schiffe platziert. Drücke 'Bereit' zum Starten.";
            }
            else
            {
                StatusMessage = $"Platziere: {SelectedShip?.Name} (Größe: {SelectedShip?.Size})";
            }
        }
        else
        {
            StatusMessage = "Schiff kann dort nicht platziert werden!";
        }
    }

    public async void AttackCell(int x, int y)
    {
        if (GamePhase != GamePhase.Playing || !IsMyTurn)
            return;

        _lastAttackX = x;
        _lastAttackY = y;
        await (_networkController?.SendTurnAsync(new TurnPacket { X = x, Y = y }) ?? Task.CompletedTask);
        IsMyTurn = false;
        StatusMessage = "Warte auf Antwort...";
    }

    private async void HandleIncomingAttack(int x, int y)
    {
        var (isHit, isSunk, ship) = _ownBoardController.TryHit(x, y);
        RefreshOwnBoardCells();

        bool gameOver = _ownBoardController.AllShipsSunk();

        await (_networkController?.SendAnswerAsync(new AnswerPacket
        {
            Hit = isHit,
            Sunk = isSunk,
            GameOver = gameOver
        }) ?? Task.CompletedTask);

        if (gameOver)
        {
            GamePhase = GamePhase.GameOver;
            StatusMessage = "Niederlage! Alle deine Schiffe wurden versenkt.";
        }
        else
        {
            IsMyTurn = true;
        }
    }

    private void HandleAttackResult(bool hit, bool sunk, bool gameOver)
    {
        var field = _enemyBoardController.Board.Fields[_lastAttackX, _lastAttackY];
        field.State = hit ? Field.FieldState.Hit : Field.FieldState.Miss;
        RefreshEnemyBoardCells();

        if (gameOver)
        {
            GamePhase = GamePhase.GameOver;
            StatusMessage = "Sieg! Du hast alle gegnerischen Schiffe versenkt!";
        }
        else if (hit)
        {
            StatusMessage = sunk ? "Schiff versenkt! Du bist nochmal dran." : "Treffer! Du bist nochmal dran.";
            IsMyTurn = true;
        }
        else
        {
            StatusMessage = "Daneben! Gegner ist dran.";
            IsMyTurn = false;
        }
    }

    private void RotateShip()
    {
        IsPlacingHorizontal = !IsPlacingHorizontal;
        StatusMessage = IsPlacingHorizontal 
            ? $"Horizontal: {SelectedShip?.Name} (Größe: {SelectedShip?.Size})" 
            : $"Vertikal: {SelectedShip?.Name} (Größe: {SelectedShip?.Size})";
    }

    private bool CanStartGame()
    {
        return GamePhase == GamePhase.PlacingShips && ShipsToPlace.Count == 0;
    }

    private async void ConfirmShipsPlaced()
    {
        GamePhase = GamePhase.WaitingForOpponent;
        StatusMessage = "Warte auf Gegner...";

        await (_networkController?.SendReadyAsync() ?? Task.CompletedTask);

        if (_opponentReady && IsHost)
        {
            BeginGame();
        }
    }

    private async void BeginGame()
    {
        var random = new Random();
        bool clientGoesFirst = random.NextDouble() > 0.5;

        await (_networkController?.SendStartPlayerAsync(
            new StartPlayerPacket { IsFirstPlayer = clientGoesFirst }) ?? Task.CompletedTask);

        IsMyTurn = !clientGoesFirst;
        GamePhase = GamePhase.Playing;
    }

    private void ResetBoard()
    {
        _ownBoardController.Board.Reset();
        ShipsToPlace.Clear();
        foreach (var ship in BoardController.CreateStandardShips())
        {
            ShipsToPlace.Add(ship);
        }
        SelectedShip = ShipsToPlace.FirstOrDefault();
        InitializeBoardCells();
        StatusMessage = $"Board zurückgesetzt. Platziere: {SelectedShip?.Name} (Größe: {SelectedShip?.Size})";
    }

    private void RefreshOwnBoardCells()
    {
        foreach (var cell in OwnBoardCells)
        {
            cell.UpdateBackground();
        }
    }

    private void RefreshEnemyBoardCells()
    {
        foreach (var cell in EnemyBoardCells)
        {
            cell.UpdateBackground();
        }
    }

    private void UpdateStatusMessage()
    {
        if (GamePhase == GamePhase.Playing)
        {
            StatusMessage = IsMyTurn ? "Du bist dran! Wähle ein Feld zum Angreifen." : "Gegner ist dran...";
        }
    }

    #endregion
}
