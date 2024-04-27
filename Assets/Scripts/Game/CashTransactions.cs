public static class CashTransactions
{
    public static void AddMoney(int _value)
    {
        if (_value < 0)
            return;
        
        SceneMediator.PlayerData._Money += _value;
    }

    public static void SpendMoney(int _value)
    {
        if (_value > SceneMediator.PlayerData._Money || _value < 0)
            return;
        
        SceneMediator.PlayerData._Money -= _value;
    }
}