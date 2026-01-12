import jakarta.xml.bind.annotation.*;
import jakarta.xml.bind.annotation.adapters.XmlJavaTypeAdapter;

import java.time.LocalDate;

public class Geburtstag {
    private LocalDate Datum;
    private String Person;

    public Geburtstag(String person, LocalDate datum) {
        Datum = datum;
        Person = person;
    }

    public Geburtstag() {}

    @XmlAttribute
    @XmlJavaTypeAdapter(value = LocalDateAdapter.class)
    public LocalDate getDatum() {
        return Datum;
    }

    public void setDatum(LocalDate datum) {
        Datum = datum;
    }

    @XmlAttribute
    public String getPerson() {
        return Person;
    }

    public void setPerson(String person) {
        Person = person;
    }
}
