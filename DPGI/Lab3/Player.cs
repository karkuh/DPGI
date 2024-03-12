namespace Lab3;

public class Player
{
    public string Name { get; set; }
    public int Win { get; set; }
    public int Lose { get; set; }
    public int Draw { get; set; }

    public Player(string name)
    {
        Name = name;
        Win = 0;
        Lose = 0;
        Draw = 0;
    }

    public void WinGame()
    {
        Win++;
    }
    
    public void LoseGame()
    {
        Lose++;
    }
    
    public void DrawGame()
    {
        Draw++;
    }

}