using UnityEngine;

public class ScrollSlot : MonoBehaviour
{
    public Scroll scroll;

    private Vector3 scrollPos = new Vector3(0f, 6f);

    public void SetScroll(Scroll scroll)
    {
        this.scroll = scroll;

        scroll.transform.SetParent(transform);
        scroll.transform.localPosition = scrollPos;
        scroll.transform.localScale = Vector3.one;
    }

    public void RemoveScroll()
    {
        scroll.gameObject.SetActive(false);

        scroll = null;
    }
}
