import java.time.LocalDate;

public class Message {
    private final int id;
    private final String message;
    private final LocalDate date;
    private final String sender;
    private final String receiver;

    public Message(int id, String message, LocalDate date, String sender, String receiver) {
        this.id = id;
        this.message = message;
        this.date = date;
        this.sender = sender;
        this.receiver = receiver;
    }

    public int getId() {
        return id;
    }

    public String getMessage() {
        return message;
    }

    public LocalDate getDate() {
        return date;
    }

    public String getSender() {
        return sender;
    }

    public String getReceiver() {
        return receiver;
    }
}
