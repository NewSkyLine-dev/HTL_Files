import jakarta.xml.bind.JAXBContext;
import jakarta.xml.bind.JAXBException;
import jakarta.xml.bind.Marshaller;
import jakarta.xml.bind.Unmarshaller;
import jakarta.xml.bind.annotation.XmlRootElement;

import java.io.File;
import java.time.LocalDate;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;

@XmlRootElement
public class GeburtstagManager {
    private List<Geburtstag> geburtstage = new LinkedList<>();

    public GeburtstagManager() {
    }

    public List<Geburtstag> getGeburtstage() {
        return geburtstage;
    }

    public void setGeburtstage(List<Geburtstag> geburtstage) {
        this.geburtstage = geburtstage;
    }

    public Map<LocalDate, List<Geburtstag>> getBirthdayByDate() {
        return geburtstage.stream()
                .collect(Collectors.groupingBy(Geburtstag::getDatum));
    }

    public void loadGeburtstage() throws JAXBException {
        File file = new File("geburtstage.xml");
        if (file.exists()) {
            JAXBContext ctx = JAXBContext.newInstance(GeburtstagManager.class);
            Unmarshaller unmarshaller = ctx.createUnmarshaller();
            GeburtstagManager geburtstagManager = (GeburtstagManager) unmarshaller.unmarshal(file);
            geburtstage = geburtstagManager.getGeburtstage();
        }
        throw new RuntimeException("File not found");
    }

    public void saveGeburtstage() throws JAXBException {
        File file = new File("geburtstage.xml");
        JAXBContext ctx = JAXBContext.newInstance(GeburtstagManager.class);
        Marshaller marshaller = ctx.createMarshaller();
        marshaller.setProperty(Marshaller.JAXB_FORMATTED_OUTPUT, true);
        marshaller.marshal(this, file);
    }
}
