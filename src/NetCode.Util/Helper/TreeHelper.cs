using System;
using System.Collections.Generic;
using System.Linq;

namespace NetCode.Util
{
    /// <summary>
    /// 树结构帮助类
    /// </summary>
    public class TreeHelper
    {
        #region 外部接口

        /// <summary>
        /// 建造树结构
        /// </summary>
        /// <param name="allNodes">所有的节点</param>
        /// <returns></returns>
        public static List<T> BuildTree<T>(List<T> allNodes) where T : TreeModel, new()
        {
            List<T> resData = new List<T>();
            var rootNodes = allNodes.Where(x => x.ParentID == "0" || x.ParentID.IsNullOrEmpty()).OrderBy(x => x.OrderNo).ToList();
            resData = rootNodes;
            resData.ForEach(aRootNode =>
            {
                if (HaveChildren(allNodes, aRootNode.ID))
                    aRootNode.Children = _GetChildren(allNodes, aRootNode);
            });

            return resData;
        }

        /// <summary>
        /// 获取所有子节点
        /// 注：包括自己
        /// </summary>
        /// <typeparam name="T">节点类型</typeparam>
        /// <param name="allNodes">所有节点</param>
        /// <param name="parentNode">父节点</param>
        /// <returns></returns>
        public static List<T> GetChildren<T>(List<T> allNodes, T parentNode) where T : TreeModel
        {
            List<T> resList = new List<T>();
            resList.Add(parentNode);
            _getChildren(allNodes, parentNode, resList);

            return resList;

            void _getChildren(List<T> _allNodes, T _parentNode, List<T> _resNodes)
            {
                var children = _allNodes.Where(x => x.ParentID == _parentNode.ID).ToList();
                _resNodes.AddRange(children);
                children.ForEach(aChild =>
                {
                    _getChildren(_allNodes, aChild, _resNodes);
                });
            }
        }

        #endregion

        #region 私有成员

        /// <summary>
        /// 获取所有子节点
        /// </summary>
        /// <typeparam name="T">树模型（TreeModel或继承它的模型）</typeparam>
        /// <param name="nodes">所有节点列表</param>
        /// <param name="parentNode">父节点ID</param>
        /// <returns></returns>
        private static List<object> _GetChildren<T>(List<T> nodes, T parentNode) where T : TreeModel, new()
        {
            Type type = typeof(T);
            var properties = type.GetProperties().ToList();
            var children = nodes.Where(x => x.ParentID == parentNode.ID).OrderBy(x => x.OrderNo).ToList();
            children.ForEach(aChildren =>
            {
                aChildren.Level = parentNode.Level + 1;
                if (HaveChildren(nodes, aChildren.ID))
                    aChildren.Children = _GetChildren(nodes, aChildren);
            });

            return children.CastToList<object>();
        }

        /// <summary>
        /// 判断当前节点是否有子节点
        /// </summary>
        /// <typeparam name="T">树模型</typeparam>
        /// <param name="nodes">所有节点</param>
        /// <param name="nodeID">当前节点ID</param>
        /// <returns></returns>
        private static bool HaveChildren<T>(List<T> nodes, string nodeID) where T : TreeModel, new()
        {
            return nodes.Exists(x => x.ParentID == nodeID);
        }

        #endregion
    }
}
