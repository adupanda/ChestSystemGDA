public class ChestController
{
    private ChestModel chestModel;
    private ChestView chestView;
    private Currency currency;
    private CurrencyDisplay currencyDisplay;

    public ChestController(ChestModel model, ChestView view, Currency currency,CurrencyDisplay currencyDisplay)
    {
        this.chestModel = model;
        this.chestView = view;
        this.currency = currency;
        this.currencyDisplay = currencyDisplay;
        chestView.Initialize(chestModel, currency,currencyDisplay);
    }

    public void CollectRewards()
    {
        chestModel.CollectRewards(currency);
        chestView.ClearChestSlot();
        // Call method to process unlock queue if needed
    }

    public bool IsUnlocking()
    {
        return chestView != null && chestView.isUnlocking;
    }

    public void StartUnlocking()
    {
        chestView.StartUnlocking();
    }
}
