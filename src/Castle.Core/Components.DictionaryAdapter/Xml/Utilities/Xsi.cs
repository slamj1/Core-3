﻿// Copyright 2004-2011 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Castle.Components.DictionaryAdapter.Xml
{
	using System;

	public static class Xsi
	{
		public static XmlName GetXsiType(this IXmlNode node)
		{
			var type = node.GetAttribute(Xsi.Type);
			if (type == null)
				return XmlName.Empty;

			var xsiType = XmlName.ParseQName(type);
			if (xsiType.NamespaceUri != null)
			{
				var namespaceUri = node.LookupNamespaceUri(xsiType.NamespaceUri);
				xsiType = xsiType.WithNamespaceUri(namespaceUri);
			}
			return xsiType;
		}

		public static void SetXsiType(this IXmlCursor node, XmlName xsiType)
		{
			if (xsiType.NamespaceUri != null)
			{
				var prefix = node.EnsurePrefix(xsiType.NamespaceUri);
				xsiType = xsiType.WithNamespaceUri(prefix);
			}
			node.SetAttribute(Xsi.Type, xsiType.ToString());
		}

		public static void SetXsiNil(this IXmlCursor node, bool nil)
		{
			string value;
			if (nil)
			{
				node.Clear();
				value = NilValue;
			}
			else value = null;
			node.SetAttribute(Xsi.Nil, value);
		}

		public static bool IsXsiNil(this IXmlNode node)
		{
			return node.GetAttribute(Xsi.Nil) == NilValue;
		}

		public const string
			Prefix         = "xsi",
			TypeLocalName  = "type",
			NilLocalName   = "nil",
			NilValue       = "true",
			NamespaceUri   = "http://www.w3.org/2001/XMLSchema-instance";

		public static readonly XmlName
			Type = new XmlName(TypeLocalName, NamespaceUri),
			Nil  = new XmlName(NilLocalName,  NamespaceUri);

		public static readonly XmlNamespaceAttribute
			Attribute = new XmlNamespaceAttribute(NamespaceUri, Prefix) { Root = true };
	}
}