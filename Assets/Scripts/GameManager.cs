using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private PlayerController player;
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private UIDocument UIDoc;

    public TurnManager TurnManager { get; private set; }

    private int _foodAmount = 100;
    private Label _foodLabel;

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
        _foodLabel.text = $"Food: {_foodAmount}";

        boardManager.Init();
        player.Spawn(boardManager, new Vector2Int(1, 1));
    }

    private void OnTurnHappen() => ChangeFood(-1);

    public void ChangeFood(int amount)
    {
        _foodAmount += amount;
        _foodLabel.text = $"Food: {_foodAmount}";
    }
}
