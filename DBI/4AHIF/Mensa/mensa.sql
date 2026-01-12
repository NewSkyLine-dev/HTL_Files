use mensa;

drop proc if exists Zutatenslite;

go
CREATE PROCEDURE Zutatenliste
    @MenueNummer INT,
    @AnzahlPortionen INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Temporäre Tabelle für die Ausgabe
    declare @ZutatenListe table (
        Zutatennummer INT,
        Bezeichnung NVARCHAR(255),
        BenötigteMenge DECIMAL(10, 2),
        Einheit NVARCHAR(50),
        FehlendeMenge DECIMAL(10, 2)
    );

    -- Cursor zur Iteration über die benötigten Zutaten des Menüs
    DECLARE ZutatenCursor CURSOR FOR
    SELECT 
        z.ZutatenNr,
        z.Bezeichnung,
        z.Einheit,
        z.aktBestand,
        SUM(sb.Menge * @AnzahlPortionen) AS BenötigteMenge
    FROM Menue_besteht_aus mb
    INNER JOIN Speise_besteht_aus sb ON mb.SpeiseNr= sb.SpeiseNr
    INNER JOIN Zutat z ON sb.Zutatennr = z.ZutatenNr
    WHERE mb.MenueNr= @MenueNummer
    GROUP BY z.ZutatenNr, z.Bezeichnung, z.Einheit, z.aktBestand;

    -- Variablen für den Cursor
    DECLARE @Zutatennummer INT;
    DECLARE @Bezeichnung NVARCHAR(255);
    DECLARE @Einheit NVARCHAR(50);
    DECLARE @Bestand DECIMAL(10, 2);
    DECLARE @BenötigteMenge DECIMAL(10, 2);
    DECLARE @FehlendeMenge DECIMAL(10, 2);

    -- Cursor öffnen
    OPEN ZutatenCursor;

    -- Lesen der ersten Zeile des Cursors
    FETCH NEXT FROM ZutatenCursor INTO @Zutatennummer, @Bezeichnung, @Einheit, @Bestand, @BenötigteMenge;

    -- Schleife über alle Datensätze
    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Fehlende Menge berechnen
        IF @Bestand < @BenötigteMenge
        BEGIN
            SET @FehlendeMenge = @BenötigteMenge - @Bestand;
        END
        ELSE
        BEGIN
            SET @FehlendeMenge = 0;
        END;

        -- Einfügen der Ergebnisse in die temporäre Tabelle
        INSERT INTO @ZutatenListe (Zutatennummer, Bezeichnung, BenötigteMenge, Einheit, FehlendeMenge)
        VALUES (@Zutatennummer, @Bezeichnung, @BenötigteMenge, @Einheit, @FehlendeMenge);

        -- Bestand aktualisieren
        UPDATE Zutat
        SET aktBestand = CASE 
                        WHEN aktBestand - @BenötigteMenge < 0 THEN 0
                        ELSE aktBestand - @BenötigteMenge
                      END
        WHERE ZutatenNr = @Zutatennummer;

        -- Nächste Zeile des Cursors lesen
        FETCH NEXT FROM ZutatenCursor INTO @Zutatennummer, @Bezeichnung, @Einheit, @Bestand, @BenötigteMenge;
    END;

    -- Cursor schließen und freigeben
    CLOSE ZutatenCursor;
    DEALLOCATE ZutatenCursor;

    -- Ausgabe der Ergebnisse
    SELECT * FROM @ZutatenListe;
END;
GO

begin try
	exec dbo.Zutatenliste 44, 1
end try
begin catch
	print error_message()
end catch