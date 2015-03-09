/* This lives on the Scroll Button.
 * It receives a RayHit() message from Ray Firer and moves the buttons
 * 
 * 
*/

using UnityEngine;
using System.Collections;

public class ScrollRayHandler : MonoBehaviour {
	
	public float yStep = 0.5f;//How fast scrolling happens. This has units distance/second
	private bool isItMyFirstTimePleaseBeGentleIfSo = true;
	private Transform[] buttonList;

	public void populateButtonList(Transform[] tempButtonList)
	{
		buttonList = tempButtonList;
	}
	public void RayHit()
	{
		//If a ray hits us, run the Move method on all the buttons
		//All the buttons!
		if (buttonList != null) {
			for (int i=0; i < buttonList.Length; i++) {

					ButtonUtil buttonUtil = buttonList [i].gameObject.GetComponentInChildren<ButtonUtil> ();
					buttonUtil.MoveButton (yStep * Time.deltaTime);
			}
		} else {
			print ("buttonList is null");
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
