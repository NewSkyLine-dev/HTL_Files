import jakarta.xml.bind.annotation.XmlAttribute;

public class CharacterFrequency implements Comparable<CharacterFrequency> {
    private String character;
    private int frequency;

    public CharacterFrequency() {
    }

    public CharacterFrequency(String character, int frequency) {
        this.character = character;
        this.frequency = frequency;
    }

    @XmlAttribute()
    public String getCharacter() {
        return character;
    }

    public void setCharacter(String character) {
        this.character = character;
    }

    @XmlAttribute()
    public int getFrequency() {
        return frequency;
    }

    public void setFrequency(int frequency) {
        this.frequency = frequency;
    }

    @Override
    public int compareTo(CharacterFrequency o) {
        return character.compareTo(o.character);
    }
}
