class Weapon
{
    private const int EmptyMagazineValue = 0;
    private const int NumberOfBulletsShooted = 1;

    private int _bullets;

    public bool CanShoot() => _bullets > EmptyMagazineValue;

    public void Shoot() => _bullets -= NumberOfBulletsShooted;
}