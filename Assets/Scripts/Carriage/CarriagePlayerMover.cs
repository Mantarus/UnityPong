using UnityEngine;

public class CarriagePlayerMover : AbstractCarriageMover
{
    protected override Vector3 Move()
    {
        var value = Input.GetAxis("Vertical");
        var position = transform.position;
        var actualSpeed = value * speed;
        return new Vector3(position.x, position.y, 
            Mathf.Clamp(position.z + actualSpeed * Time.deltaTime, -limit, limit));
    }
}
