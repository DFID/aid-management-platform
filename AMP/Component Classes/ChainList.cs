using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;
using MoreLinq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace AMP.Component_Classes
{
    public class ChainList
    {
        private List<DeliveryChain> _deliverychain; 
        public ChainList(List<DeliveryChain> deliverychains )
        {
            _deliverychain = deliverychains;
        }

        public List<DeliveryChain> ReturnDeliveryChains()
        {
            return _deliverychain.ToList();
        } 

        public List<DeliveryChain> FirstTierPartners()
        {
            //return _deliverychain.Where(x => x.ChainID == 1).ToList();
            return _deliverychain.Where(x => x.ChainID.Length == 1).ToList();
        }

        public string NextChainIDFromPartnerHierarchy(Int32 parentId)
        {
            DeliveryChain parent = _deliverychain.FirstOrDefault(x => x.ID == parentId);

            if (_deliverychain.Any(x => x.ParentNodeID == parent.ID && (x.ParentID != x.ChildID)))
            {
                //Parent has children. Get the highest chainId and increment - ChainID is a string.
                
                Int64 chainId = Int64.Parse(_deliverychain.Where(x => x.ParentNodeID == parent.ID).MaxBy(y => y.ChainID).ChainID);
                return (chainId + 1).ToString();

            }
            else
            {


                //Add 01 to the end of the parent chain ID.  
                return parent.ChainID + "01";
            }
        }

        public string NextFirstTierPartnerChainID()
        {
            
            if (_deliverychain.Any(x => x.ChainID.Length == 1))
            {
                List<DeliveryChain> TierOnePartners = _deliverychain.Where(x => x.ChainID.Length == 1).ToList();
                return (Int64.Parse(TierOnePartners.MaxBy(y => y.ChainID).ChainID) + 1).ToString();
            }
            else
            {
                return "1";
            }
        }

        public DeliveryChain FindDeliveryChainById(Int32 id)
        {
            return  _deliverychain.FirstOrDefault(x => x.ID == id);
        }

        public DeliveryChain FindParentByChildId(Int32 id)
        {
            DeliveryChain child = _deliverychain.FirstOrDefault(x => x.ID == id);
            DeliveryChain parent = _deliverychain.FirstOrDefault(y => y.ID == child.ParentNodeID);

            return parent;
        }

        public void LinkChildrenToNewDeliveryChain(DeliveryChain chain, string parentChainId)
        {
            //_deliverychain.Where(x => x.ParentID == chain.ChildID && x.ChainID.Contains(parentChainId)).Select(c =>
            //{
            //    c.ParentID = chain.ParentID;
            //    return c;
            //}).ToList();

            //// Check if chain to be deleted is a 1st tier partner
            //if (chain.ID == chain.ParentNodeID && chain.ParentID == chain.ChildID)
            //{
            //    _deliverychain.Where(x => x.ParentNodeID == chain.ID).Select(c =>
            //    {
            //        c.ParentID = c.ChildID;
            //        c.ParentType = c.ChildType;
            //        c.ParentNodeID = c.ID;

            //        return c;
            //    }).ToList();
            //}

            //else
            //{
                _deliverychain.Where(x => x.ParentNodeID == chain.ID).Select(c =>
                {
                    c.ParentID = chain.ParentID;
                    c.ParentNodeID = chain.ParentNodeID;
                    return c;
                }).ToList();
            //}
        }

        public void PromoteChildrenToFirstTier(DeliveryChain chain)
        {
            string highestFirstTier;

            if (_deliverychain.Any(x => x.ChainID.Length < 3))
            {
                List<DeliveryChain> TierOnePartners = _deliverychain.Where(x => x.ChainID.Length < 3).ToList();
                highestFirstTier = (Int64.Parse(TierOnePartners.MaxBy(y => y.ChainID).ChainID) + 1).ToString();
               
            }
            else
            {
                highestFirstTier = "1";
            }

            //_deliverychain.Where(x => x.ParentNodeID == chain.ID).Select(c =>
            //    {
            //        c.ParentID = c.ChildID;
            //        c.ParentType = c.ChildType;
            //        c.ParentNodeID = c.ID;
            //        c.ChainID = highestFirstTier;
            //        return c;
            //    });
            List<DeliveryChain> deletedChildren = _deliverychain.Where(x => x.ParentNodeID == chain.ID && x.ParentNodeID != x.ID).ToList();

            foreach (DeliveryChain child in deletedChildren)
            {
                _deliverychain.Remove(child);
                child.ParentID = child.ChildID;
                child.ParentType = child.ChildType;
                child.ParentNodeID = child.ID;
                child.ChainID = highestFirstTier;
                highestFirstTier = (Int64.Parse(highestFirstTier) + 1).ToString();
                _deliverychain.Add(child);
            }

        }

        public List<DeliveryChain> FindChildrenByParentId(int parentId)
        {
            return _deliverychain.Where(x => x.ParentID == parentId).ToList();
        }

        public void DeleteDeliveryChain(int deliveryChainId, string parentChainId)
        {
       //     _deliverychain.FirstOrDefault(x => x.ID == deliveryChainId && x.ChainID.Contains(parentChainId)).Status = "N";
            _deliverychain.FirstOrDefault(x => x.ID == deliveryChainId).Status = "N";
        }

        public void DeleteDeliveryChainAndChildren(int deliveryChainId, string chainId)
        {
         List< DeliveryChain> chainstoDelete = _deliverychain.Where(x => x.ChainID.StartsWith(chainId)).ToList();

            for (int i = 0; i < chainstoDelete.Count; i++)
            {
             
                _deliverychain.First(x => x.ID == chainstoDelete[i].ID).Status="N" ;              
            }
          
        }
        public void UpdateChainIDsAfterChangingTheChain()
        {
            _deliverychain.Select(c =>
            {
                c.ParentID = 0;
                return c;
            });

            List<DeliveryChain> _firstTierDeliveryChains = _deliverychain.Where(x => x.ChainID.Length == 1 && x.Status == "A").OrderBy(c => c.ID).ToList();

            //int j = 1;
            for (int i = 0; i < _firstTierDeliveryChains.Count; i++)
            {
                int parentId = _deliverychain.First(x=> x.ID == _firstTierDeliveryChains[i].ID).ParentID;                       
                int parentNodeId = _deliverychain.First(x => x.ID == _firstTierDeliveryChains[i].ID).ID;
               
                _deliverychain.First(x => x.ID == _firstTierDeliveryChains[i].ID).ChainID = (i + 1).ToString();
                _deliverychain.First(y => y.ID == _firstTierDeliveryChains[i].ID).ParentNodeID = parentNodeId;
                UpdateChainID(parentId, parentNodeId, (i + 1).ToString());
                //j++;
            }
        }

        public void UpdateChainID(int parentId, int? parentNodeId, string parentChainId)
        {
            
            string chainId = "";
         

            if (_deliverychain.Any(x => x.ParentID == parentId && x.ChildID != parentId && x.ParentNodeID == parentNodeId && x.Status == "A"))
            {
                List<DeliveryChain> children = _deliverychain.Where(x => x.ParentID == parentId && x.ChildID != parentId && x.ParentNodeID == parentNodeId && x.Status == "A").ToList();
                if (children.Any())
                {
                    for (int i = 0; i < children.Count; i++)
                    {
                        chainId = string.Format("{0}0{1}", parentChainId, (i + 1).ToString());
                        _deliverychain.FirstOrDefault(x => x.ID == children[i].ID).ChainID = chainId;
                        _deliverychain.FirstOrDefault(x => x.ID == children[i].ID).ParentNodeID = parentNodeId;
                        UpdateChainID(children[i].ChildID, children[i].ID, chainId);
                    }
                }
            }

        }

    }
}