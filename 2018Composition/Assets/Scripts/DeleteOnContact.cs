using UnityEngine;


public class DeleteOnContact : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        //detect when the sprite is moved out of the grid and remove it from the list
        if (other.gameObject.name=="Move")
        {
            other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            Destroy(other);
        }
    }
}
