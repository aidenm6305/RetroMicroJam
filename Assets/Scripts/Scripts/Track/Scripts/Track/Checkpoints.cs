using UnityEngine;
public class CheckPoint : MonoBehaviour
{
    int index;

    TrackManager trackManagerParent;

    public void Setup(int index, TrackManager trackManagerParent)
    {
        this.index = index;
        this.trackManagerParent = trackManagerParent;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            trackManagerParent.CheckPointPassed(this);
        }
    }

    public int GetIndex()
    {
        return index;
    }


}