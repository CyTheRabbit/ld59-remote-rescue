namespace Lag
{
    public class Bot : PawnBase
    {
        public Projectile ProjectilePrefab;

        public override void Fire()
        {
            var projectile = Instantiate(ProjectilePrefab);
            projectile.transform.position = transform.position;
            projectile.Direction = Input.Direction;
            projectile.gameObject.SetActive(true);
        }
    }
}