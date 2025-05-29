using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public BoardManager BoardManager { get => boardManager; }

    [SerializeField] private PlayerController player;
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private UIDocument UIDoc;
    
    public TurnManager TurnManager { get; private set; }

    private int _foodAmount = 20;
    private VisualElement _gameOverPanel;
    private Label _foodLabel;
    private Label _gameOverMassage;

    private int _currentLevel = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        TurnManager = new TurnManager();
        TurnManager.OnTick += OnTurnHappen;

        _foodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");

        _gameOverPanel = UIDoc.rootVisualElement.Q<VisualElement>("GameOverPanel");
        _gameOverMassage = _gameOverPanel.Q<Label>("GameOverMassage");

        _gameOverPanel.style.visibility = Visibility.Hidden;

        InitLevel();
    }

    private void OnTurnHappen() => ChangeFood(-1);

    public void ChangeFood(int amount)
    {
        _foodAmount += amount;
        _foodLabel.text = $"Food: {_foodAmount}";

        if (_foodAmount <= 0)
        {
            player.GameOver();
            _gameOverPanel.style.visibility = Visibility.Visible;
            _gameOverMassage.text = $"GameOver! \n\n You traveled throught {_currentLevel} levels";
        }
    }

    public void InitLevel()
    {
        _foodLabel.text = $"Food: {_foodAmount}";
        boardManager.Init();
        player.Spawn(boardManager, new Vector2Int(1, 1));
    }

    public void NewLevel()
    {
        BoardManager.CleanLevel();
        InitLevel();
        _currentLevel += 1;
    }

    public void StartNewGame()
    {
        _gameOverPanel.style.visibility = Visibility.Hidden;
        _currentLevel = 0;
        _foodAmount = 20;

        BoardManager.CleanLevel();
        InitLevel();
    }
}
