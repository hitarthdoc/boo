﻿#region license
// Copyright (c) 2003, 2004, 2005 Rodrigo B. de Oliveira (rbo@acm.org)
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
//     * Neither the name of Rodrigo B. de Oliveira nor the names of its
//     contributors may be used to endorse or promote products derived from this
//     software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion

using System.Collections.Generic;

namespace Boo.Lang.Compiler.TypeSystem.Core
{
	public class NamespaceDelegator : AbstractNamespace
	{
		private INamespace _parent;
		
		private List<INamespace> _namespaces = new List<INamespace>();
		
		public NamespaceDelegator(INamespace parent, params INamespace[] namespaces)
		{
			_parent = parent;
			_namespaces.ExtendUnique(namespaces);
		}
		
		public override INamespace ParentNamespace
		{
			get { return _parent; }
		}

		public void DelegateTo(INamespace ns)
		{
			_namespaces.AddUnique(ns);
		}

		public override bool Resolve(ICollection<IEntity> resultingSet, string name, EntityType typesToConsider)
		{
			bool found = false;
			foreach (INamespace @delegate in Delegates)
				if (@delegate.Resolve(resultingSet, name, typesToConsider))
					found = true;
			return found;
		}

		protected IEnumerable<INamespace> Delegates
		{
			get { return _namespaces; }
		}

		public override IEnumerable<IEntity> GetMembers()
		{
			foreach (INamespace ns in _namespaces)
				foreach (IEntity member in ns.GetMembers())
					yield return member;
		}
	}
}