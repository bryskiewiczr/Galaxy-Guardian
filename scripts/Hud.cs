using Godot;

public partial class Hud : Control {
    private Label _scoreLabel;
    private Label _livesLabel;

    public override void _Ready() {
        _scoreLabel = GetNode<Label>("ScoreLabel");
        _livesLabel = GetNode<Label>("LivesLabel");
    }

    public void SetScoreLabel(int score) {
        _scoreLabel.Text = $"Score: {score.ToString()}";
    }

    public void SetLivesLabel(int lives) {
        _livesLabel.Text = $"Lives: {lives.ToString()}";
    }
}
