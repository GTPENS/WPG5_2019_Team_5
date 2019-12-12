public class Config {
    public int gold = 200;

    private static Config instance = new Config();

    public static Config getInstance {
        get { return instance; }
    }
}