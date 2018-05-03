﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

namespace MiniMap
{
    public class NodeWorld : MonoBehaviour
    {
        [HideInInspector]
        public string itemInfo;
        [HideInInspector]
        public Sprite icon;
        [SerializeField][HideInInspector]
        private List<int> mapKeys = new List<int>();
        [HideInInspector]
        public Transform posHolder;
        [HideInInspector]
        public bool updateIcon;
        [HideInInspector]
        public bool updateIconRot;
        [SerializeField][HideInInspector]
        private string iconPrefab;
        [HideInInspector]
        public float updateTime = 3;

        private float timer;
        private Dictionary<MapItem, NodeIcon> mapDic = new Dictionary<MapItem, NodeIcon>();// nodeicon;

        void Awake()
        {
            if (posHolder == null) posHolder = transform;
            MiniMapSystem.Instence.Regist(this);
        }
        void OnDestroy()
        {
            MiniMapSystem.Instence.Remove(this);
        }
        public void Update()
        {
            if (updateIcon && mapDic.Count > 0 && (timer += Time.deltaTime) > updateTime)
            {
                timer = 0;
                UpdateNodeIcons();
            }
        }

        public void RegistNodeIcon(MapItem map)
        {
            if(!mapDic.ContainsKey(map)){
                mapDic.Add(map, CreateNodeIcon(map));
            }
            UpdateNodeIcons();
        }

        public void RemoveNodeIcon(MapItem map)
        {
            if(mapDic.ContainsKey(map))
            {
                var mapIcon = mapDic[map];
                if(mapIcon != null && mapIcon.gameObject != null){
                    Destroy(mapIcon.gameObject);
                }
                mapDic.Remove(map);
            }
        }

        public bool WillShowOnMap(int key)
        {
            if (mapKeys.Contains(key))
            {
                return true;
            }
            if(mapKeys.Count == 0)
            {
                return true;
            }
            return false;
        }

        private NodeIcon CreateNodeIcon(MapItem map)
        {
            var nodeicon = map.CreateIcon(iconPrefab);
            nodeicon.InitICON(this, map);
            nodeicon.transform.position = map.GetUIPositon(posHolder.position);
            return nodeicon;
        }

        private void UpdateNodeIcons()
        {
            foreach (var item in mapDic)
            {
                var map = item.Key;
                var nodeicon = item.Value;
                if(nodeicon != null && nodeicon.gameObject != null)
                {
                    if (nodeicon.gameObject.activeInHierarchy)
                    {
                        nodeicon.transform.position = map.GetUIPositon(posHolder.position);
                        if(updateIconRot)
                        {
                            nodeicon.transform.eulerAngles = new Vector3(0, 0, -transform.eulerAngles.y);
                        }
                    }
                }
              
            }
        }

    }
}