public interface IDamagable
{
    byte ID { get; set; }
    void TakeDamage(float damage);
}