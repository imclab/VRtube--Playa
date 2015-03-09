//Lives on OVRPlayerController
/// <summary>
/// Cursor ray interacter. Lives on OVRPlayerController (but might change)
/// Shoots rays from objects. If it's the right kind of object, tell it to do something.
/// </summary>
//TODO
/// Try sending it out from different objects
/// Try having it working only at certain distances
using UnityEngine;
using System.Collections;

public class RayFirer : MonoBehaviour {
	public GameObject rayCamera; //This is where the Ray is being shot from
	public Transform cursorPrefab;
	public Transform canvasObject; //this is to get the right rotation
	public float cursorHeight = -0.03f; //this is the height of the cursor above the button
	private Transform cursorInstance;
	private RaycastHit hit; //This is whatever the ray hits
	private Ray firingRay; //The ray that is shot out

	void Start () {
		//instantiate the cursor, but make it inactive to start out with
		cursorInstance = (Transform)Instantiate(cursorPrefab, new Vector3(0, 0, 0), Quaternion.identity);
		cursorInstance.gameObject.SetActive (false);

		//Rotate the cursor in the same way as the canvas is.
		//Not sure why this is necessary, but whatevs.
		cursorInstance.rotation = canvasObject.rotation;
	}
	
	// Update is called once per frame which on my shitty ass computer is a solid 10 times a second...
	void Update () {
		
		//Shoot out ray
		firingRay = new Ray (rayCamera.transform.position, rayCamera.transform.forward);
		//Debug.DrawRay (rayCamera.transform.position, rayCamera.transform.forward * 1000);
		
		//Check if ray has hit something
		if (Physics.Raycast (firingRay, out hit)) {

			//Check if ray has hit a UI element
			if (hit.collider.gameObject.layer==5) { //This tests if the user is looking at the UI layer
					
					
				if (hit.collider.tag.Equals ("UIbutton")) {
					//Tell that object it's been hit
					hit.collider.SendMessage ("RayHit");//if the ray has hit the UI
				}
					//Get location of where the user is looking
					Vector3 cursorPosition = hit.point;
					//make the cursor at a position just above where ever it hit
					cursorInstance.position = cursorPosition + (cursorInstance.forward / cursorInstance.forward.magnitude) * cursorHeight;
					//Show the cursor now that the user is looking at soomething
					cursorInstance.gameObject.SetActive (true);

			} 
			else {
				//Don't show the cursor if she isn't looking at a UI element
				cursorInstance.gameObject.SetActive (false);
			}

		} else {
			//Don't show the cursor if she isn't looking at anything
			cursorInstance.gameObject.SetActive (false);
		}






	}
}

/* This file is part of VRtube, Playa.

    VRtube, Playa is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    VRtube, Playa is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with VRtube, Playa.  If not, see <http://www.gnu.org/licenses/>.
*/
