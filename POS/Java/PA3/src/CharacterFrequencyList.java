import jakarta.xml.bind.annotation.*;

import java.util.*;

@XmlRootElement
public class CharacterFrequencyList {
    private Set<CharacterFrequency> characterFrequencyList = new TreeSet<>();
    private List<String> textList = new ArrayList<>();

    public CharacterFrequencyList() {
    }

    @XmlElement()
    public Set<CharacterFrequency> getCharacterFrequencyList() {
        return characterFrequencyList;
    }

    public void setCharacterFrequencyList(Set<CharacterFrequency> characterFrequencyList) {
        this.characterFrequencyList = characterFrequencyList;
    }

    @XmlElement()
    public List<String> getTextList() {
        return textList;
    }

    public void setTextList(List<String> textList) {
        this.textList = textList;
    }

    public void addText(String text) {
        textList.add(text);
    }
}
