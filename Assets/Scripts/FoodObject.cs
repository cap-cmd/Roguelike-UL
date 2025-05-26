using UnityEngine;

public class FoodObject : CellObject
{
    [SerializeField] private int amountGranted;
    public override void PlayerEntered()
    {
        Destroy(gameObject);
        GameManager.Instance.ChangeFood(amountGranted);
    }
}
