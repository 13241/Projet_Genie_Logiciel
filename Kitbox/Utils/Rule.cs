using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox
{
    public class Rule
    {
        private Delegate action; //method that execute the rule
        private OrderedDictionary args; //stocked arguments
        private Type Ttrigger; //type of the property whose modification can trigger the rule
        private Type Ttarget; //type of the object whose modified property can trigger the rule

        public Rule(Delegate action, OrderedDictionary args, Type Ttrigger = null, Type Ttarget = null)
        {
            this.action = action;
            this.args = args;
            this.Ttrigger = Ttrigger;
            this.Ttarget = Ttarget;
        }

        /// <summary>
        /// Execute the Rule Delegate with the args missing inputs
        /// </summary>
        /// <param name="args"></param>
        public void Execute(Dictionary<object, object> args)
        {
            OrderedDictionary param = new OrderedDictionary();
            foreach(DictionaryEntry entry in this.args)
            {
                if(entry.Value == null)
                {
                    param.Add(entry.Key, args[entry.Key]);
                }
                else
                {
                    param.Add(entry.Key, entry.Value);
                }
            }
            object[] param_args = new object[this.args.Count];
            param.Values.CopyTo(param_args, 0);
            action.DynamicInvoke(param_args);
        }
        
        public Type Target
        {
            get { return Ttarget; }
        }

        public Type Trigger
        {
            get { return Ttrigger; }
        }
    }
}
