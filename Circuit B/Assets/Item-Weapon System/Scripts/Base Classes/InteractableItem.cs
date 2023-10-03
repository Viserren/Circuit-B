public abstract class InteractableItem : Item
{
    public override void Pickup()
    {
        throw new System.NotImplementedException();
    }

    public abstract void Use();
}
