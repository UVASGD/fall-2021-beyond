/**
 * A general purpose timer that counts down from a set value to 0.
 */
public class Timer
{
    // automatically generates getters/setters
    public float maxValue { get; private set; }
    public float value { get; private set; }
    public bool started { get; set; }

    public Timer(float startValue)
    {
        this.maxValue = startValue;
        this.value = startValue;
    }

    public bool IsReady()
    {
        return value <= 0;
    }

    public void Reset()
    {
        value = maxValue;
    }

    public void CountDown(float dt)
    {
        value -= dt;
    }
}

