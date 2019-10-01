public class Config {
    public int gold = 2000;
    public int turn = 0;

    private static Config instance = new Config();

    public static Config getInstance {
        get { return instance; }
    }
}