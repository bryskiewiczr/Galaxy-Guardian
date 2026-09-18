using Godot;

public partial class GameOver : Control {

    private Button _restartButton;
    private Button _quitButton;

    public override void _Ready() {
        _restartButton = GetNode<Button>("Panel/RetryButton");
        _quitButton = GetNode<Button>("Panel/QuitButton");

        _restartButton.Pressed += RestartGame;
        _quitButton.Pressed += QuitGame;
    }

    private void RestartGame() {
        GetTree().ReloadCurrentScene();
    }

    private void QuitGame() {
        GetTree().Quit();
    }

}
