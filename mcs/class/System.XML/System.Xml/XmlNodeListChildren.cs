//
// System.Xml.XmlNodeList
//
// Author:
//   Kral Ferch <kral_ferch@hotmail.com>
//
// (C) 2002 Kral Ferch
//

using System;
using System.Collections;

namespace System.Xml
{
	public class XmlNodeListChildren : XmlNodeList
	{
		#region Enumerator

		private class Enumerator : IEnumerator
		{
			XmlNode parent;
			XmlLinkedNode currentChild;
			bool passedLastNode;

			internal Enumerator (XmlNode parent)
			{
				currentChild = null;
				this.parent = parent;
				passedLastNode = false;
			}

			public virtual object Current {
				get {
					if ((currentChild == null) ||
						(parent.LastLinkedChild == null) ||
						(passedLastNode == true))
						throw new InvalidOperationException();

					return currentChild;
				}
			}

			public virtual bool MoveNext()
			{
				bool movedNext = true;

				if (parent.LastLinkedChild == null) {
					movedNext = false;
				}
				else if (currentChild == null) {
					currentChild = parent.LastLinkedChild.NextLinkedSibling;
				}
				else {
					if (Object.ReferenceEquals(currentChild, parent.LastLinkedChild)) {
						movedNext = false;
						passedLastNode = true;
					}
					else {
						currentChild = currentChild.NextLinkedSibling;
					}
				}

				return movedNext;
			}

			public virtual void Reset()
			{
				currentChild = null;
			}
		}

		#endregion

		#region Fields

		XmlNode parent;

		#endregion

		#region Constructors
		public XmlNodeListChildren(XmlNode parent)
		{
			this.parent = parent;
		}

		#endregion

		#region Properties

		public override int Count {
			get {
				int count = 0;

				if (parent.LastLinkedChild != null) {
					XmlLinkedNode currentChild = parent.LastLinkedChild.NextLinkedSibling;
					
					count = 1;
					while (!Object.ReferenceEquals(currentChild, parent.LastLinkedChild)) {
						currentChild = currentChild.NextLinkedSibling;
						count++;
					}
				}

				return count;
			}
		}

		#endregion

		#region Methods

		public override IEnumerator GetEnumerator ()
		{
			return new Enumerator(parent);
		}

		public override XmlNode Item (int index)
		{
			XmlNode requestedNode = null;

			// Instead of checking for && index < Count which has to walk
			// the whole list to get a count, we'll just keep a count since
			// we have to walk the list anyways to get to index.
			if ((index >= 0) && (parent.LastLinkedChild != null)) {
				XmlLinkedNode currentChild = parent.LastLinkedChild.NextLinkedSibling;
				int count = 0;

				while ((count < index) && !Object.ReferenceEquals(currentChild, parent.LastLinkedChild)) 
				{
					currentChild = currentChild.NextLinkedSibling;
					count++;
				}

				if (count == index) {
					requestedNode = currentChild;
				}
			}

			return requestedNode;
		}

		#endregion
	}

	
}
