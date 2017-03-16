using UnityEngine;

public class Player2AnimScript : MonoBehaviour {
    Animator anim;
    public float SliceAgainTime = 0.3f;
    float SliceAgainElapsed = 0.0f;
    private AudioSource lightsaberSound;

	void Start () {
        anim = GetComponent<Animator>();
        lightsaberSound = GetComponent<AudioSource>();
	}
	
	void Update () {
        TrySlice();
    }

    void TrySlice()
    {
        if (Input.GetAxis("Fire2P2") != 0)
        {
            if (Time.time >= SliceAgainElapsed)
            {
                Slice();
                SliceAgainElapsed = Time.time + SliceAgainTime;
            }
        }
    }

    void Slice()
    {
        //if (anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        //{
        //    var objectPos = Camera.main.WorldToScreenPoint(GetComponent<Transform>().position);
        //    var targetX = Input.mousePosition.x - objectPos.x;
        //    var targetY = Input.mousePosition.y - objectPos.y;
        //    var angle = Mathf.Atan2(targetY, targetX) * Mathf.Rad2Deg;
        //    while(angle < 0) { angle += 360; }
        //    angle = angle % 360;

        //    if (angle <= 25 || angle >= 335) { anim.Play("swipeRight"); }
        //    else if (angle <= 65) { anim.Play("swipeUpRight"); }
        //    else if (angle <= 115) { anim.Play("swipeUp"); }
        //    else if (angle <= 155) { anim.Play("swipeUpLeft"); }
        //    else if (angle <= 205) { anim.Play("swipeLeft"); }
        //    else if (angle <= 245) { anim.Play("swipeDownLeft"); }
        //    else if (angle <= 295) { anim.Play("swipeDown"); }
        //    else if (angle <= 335) { anim.Play("swipeDownRight"); }
        //    else { print("Slice(): Defaulted...[" + angle.ToString() + "]"); }
        //    //print("[" + angle.ToString() + "]");
        //}

        anim.Play("swipeUp");
        lightsaberSound.Play();
    }
}
