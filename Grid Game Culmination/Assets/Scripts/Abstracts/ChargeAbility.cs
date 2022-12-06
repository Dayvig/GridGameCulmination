namespace DefaultNamespace
{
    public interface ChargeAbility
    {
        public void spendCharge(BaseBehavior initiator);
        public void setCharges(BaseBehavior initiator, int newCharges);
        public int getCharges(BaseBehavior initiator);
    }
}