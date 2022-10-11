public void SetEnable()
{
    _enable = true;
    _effects.StartEnableAnimation();
}

public void SetDisable()
{
    _enabel = false;
    _pool.Free(this);
}