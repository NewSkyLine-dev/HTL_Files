import jakarta.xml.bind.annotation.*;

import java.util.*;

@XmlRootElement
@XmlAccessorType(XmlAccessType.FIELD)
public class Sudoku {
    @XmlElement(name="feld")
    private List<Field> fields = new ArrayList<>(81);

    public Sudoku() {}

    public Sudoku(List<Field> fields) {
        this.fields = fields;
    }

    public List<Field> getFields() {
        return fields;
    }

    public void setFields(List<Field> fields) {
        this.fields = fields;
    }
}