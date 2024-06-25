using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MTAssets.EasyMinimapSystem
{
    public class GameController : MonoBehaviour
    {
        //Private cache
        private float beforeFullscreenGlobalSizeMultiplier = -1;

        //Private cache to minimap renderer events
        private int clicksToCreateMarkInMinimap = 0;
        private Vector3 lastWorldPosClickInMinimap = Vector3.zero;
        private Dictionary<MinimapItem, Vector3> allMinimapItemsAndOriginalSizes = new Dictionary<MinimapItem, Vector3>();

        //Public variables
        public GameObject fullScreenMapObj;
        public PlayerScript player;
        public MinimapCamera playerMinimapCamera;
        public MinimapItem marker;
        public MinimapItem cursor;
        public MinimapItem playerFieldOfView;

        //On update

        void Update()
        {
        }


        public void OpenFullscreenMap()
        {
            fullScreenMapObj.SetActive(true);
            playerMinimapCamera.gameObject.SetActive(true);
            beforeFullscreenGlobalSizeMultiplier = MinimapDataGlobal.GetMinimapItemsSizeGlobalMultiplier();
            MinimapDataGlobal.SetMinimapItemsSizeGlobalMultiplier(1.5f);
            if (cursor != null)
                cursor.gameObject.SetActive(true);

        }

        public void CloseFullscreenMap()
        {
            fullScreenMapObj.SetActive(false);
            playerMinimapCamera.gameObject.SetActive(false);
            MinimapDataGlobal.SetMinimapItemsSizeGlobalMultiplier(beforeFullscreenGlobalSizeMultiplier);
            if (cursor != null)
                cursor.gameObject.SetActive(false);
        }

        public void OnClickInMinimapRendererArea(Vector3 clickWorldPos, MinimapItem clickedMinimapItem)
        {
            if (clicksToCreateMarkInMinimap == 0)
                StartCoroutine(OnClickInMinimapRendererArea_DoubleClickRoutine());
            clicksToCreateMarkInMinimap += 1;
            lastWorldPosClickInMinimap = clickWorldPos;
            if (clickedMinimapItem != null)
                Debug.Log("You clicked on Minimap Item \"" + clickedMinimapItem.gameObject.name + "\".");
        }

        IEnumerator OnClickInMinimapRendererArea_DoubleClickRoutine()
        {
            int milisecondsPassed = 0;

            while (enabled)
            {
                if (milisecondsPassed >= 25) //<-- if is passed 25ms, reset the counter of clicks and break the loop
                {
                    clicksToCreateMarkInMinimap = 0;
                    break;
                }

                if (clicksToCreateMarkInMinimap >= 2)
                {
                    marker.gameObject.SetActive(true);
                    marker.transform.position = lastWorldPosClickInMinimap;
                    clicksToCreateMarkInMinimap = 0;
                    break;
                }

                yield return new WaitForSecondsRealtime(0.001f); //<-- 0.001 is 1ms
                milisecondsPassed += 1;
            }
        }

        public void OnDragInMinimapRendererArea(Vector3 onStartThisDragWorldPos, Vector3 onDraggingWorldPos)
        {
            Vector3 deltaPositionToMoveMap = (onDraggingWorldPos - onStartThisDragWorldPos) * -1.0f;
            playerMinimapCamera.transform.position += (deltaPositionToMoveMap * 10.0f * Time.deltaTime);
        }

        public void OnOverInMinimapRendererArea(bool isOverMinimapRendererArea, Vector3 mouseWorldPos, MinimapItem overMinimapItem)
        {
            if (isOverMinimapRendererArea == true)
            {
                cursor.gameObject.SetActive(true); //<- "Raycast Target" of this Minimap Item, is off

                cursor.gameObject.transform.position = mouseWorldPos;

                foreach (var key in allMinimapItemsAndOriginalSizes)
                {
                    if (overMinimapItem != null && key.Key == overMinimapItem)
                        continue;

                    key.Key.sizeOnMinimap = key.Value;
                }

                MinimapItem[] allMinimapItems = cursor.GetListOfAllMinimapItemsInThisScene();
                for (int i = 0; i < allMinimapItems.Length; i++)
                {
                    MinimapItem item = allMinimapItems[i];
                    if (item == null)
                        continue;
                    if (allMinimapItemsAndOriginalSizes.ContainsKey(item) == false)
                        allMinimapItemsAndOriginalSizes.Add(item, item.sizeOnMinimap);
                }

                if (overMinimapItem != null && overMinimapItem.sizeOnMinimap != (allMinimapItemsAndOriginalSizes[overMinimapItem] * 3.0f))
                    overMinimapItem.sizeOnMinimap = overMinimapItem.sizeOnMinimap * 3.0f;
            }
        }
    }
}