﻿using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.SessionState;

namespace CarRental.Tests.WEB.Fakes
{
    public class FakeHttpSessionState : HttpSessionStateBase
    {
        private readonly SessionStateItemCollection _sessionItems;

        public FakeHttpSessionState(SessionStateItemCollection sessionItems)
        {
            if(sessionItems != null)
                _sessionItems = sessionItems;
            else
                _sessionItems = new SessionStateItemCollection();
        }

        public override void Add(string name, object value)
        {
            _sessionItems[name] = value;
        }

        public override int Count => _sessionItems.Count;

        public override IEnumerator GetEnumerator()
        {
            return _sessionItems.GetEnumerator();
        }

        public override NameObjectCollectionBase.KeysCollection Keys => _sessionItems.Keys;

        public override object this[string name]
        {
            get
            {
                return _sessionItems[name];
            }
            set
            {
                _sessionItems[name] = value;
            }
        }

        public override object this[int index]
        {
            get
            {
                return _sessionItems[index];
            }
            set
            {
                _sessionItems[index] = value;
            }
        }

        public override void Remove(string name)
        {
            _sessionItems.Remove(name);
        }
    }
}
