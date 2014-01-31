/// <reference path="json2.min.js"/>

var JSMController = (function (window, document) {
	'use strict';
	var objController = {};
	// private properties
	var VARCONTAINERID = "JSMVariables";
	var FUNCCONTAINERID = "JSMFunctions";
	var PageLoadEvent = { Init: 0, Ready: 1 };
	var arrQueue = [];

	// private functions
	function GetEmbeddedJson(strId) {
		var strJson = document.getElementById(strId).innerHTML;
		var bolComment = (strJson.indexOf("<") === 0) || (strJson.indexOf("&lt;") === 0); //check if we are in another browser than IE
		if (!bolComment) // IE detected, return Json
			return strJson.replace(/\&amp;/g, "&"); //ie is messing with some templates
		var bolLtEntity = (strJson.indexOf("&lt;") === 0); // Fix for Firefox 3.6
		return strJson.substring(bolLtEntity ? 20 : 14, (strJson.length - (bolLtEntity ? 18 : 12))); //return substring without html comment tags
	};
	function ParseCodeBehindJson(strJson) {
		return strJson ? JSON.parse(strJson) : null;
	};
	function FillCodeBehindJsVariables(dictVariables) {
		if (!dictVariables)
			return;
		for (var strVariableName in dictVariables) {
			var objValue = dictVariables[strVariableName];
			var arrVariableName = strVariableName.split(".");
			var objCurrent = window;
			// If the variable contains '.' we have to get or create the literals iteratively
			for (var i = 0; i < arrVariableName.length - 1; ++i) {
				//literal not found --> create new one
				if (!objCurrent[arrVariableName[i]]) {
					objCurrent[arrVariableName[i]] = {};
				}
				objCurrent = objCurrent[arrVariableName[i]];
			}
			// finally set the value
			objCurrent[arrVariableName[arrVariableName.length - 1]] = objValue;
		}
	};
	function FillCodeBehindJavaScriptFunctions(dictFunctions, enmPageLoadEvent) {
		if (!dictFunctions)
			return;
		for (var i = 0; i < dictFunctions.length; ++i) {
			var objFunctionContainer = dictFunctions[i];
			var arrFunctionName = objFunctionContainer.Function.split(".");
			var delFunction = window;
			// If the function name contains '.' we have to get the function iteratively
			for (var j = 0; j < arrFunctionName.length; ++j) {
				delFunction = delFunction[arrFunctionName[j]];
			}
			if (typeof delFunction !== "function")
				throw new Error(objFunctionContainer.Function + " is not a valid function.");
			// store the function reference in the queue
			arrQueue.push({ Type: enmPageLoadEvent, Function: delFunction, Parameters: objFunctionContainer.Parameters });
		}
	};
	function Initialize(dicJavaScriptVariables, dicJavaScriptFunctions, enmPageLoadEvent) {
		FillCodeBehindJsVariables(dicJavaScriptVariables);
		FillCodeBehindJavaScriptFunctions(dicJavaScriptFunctions, enmPageLoadEvent);
		RunQueue(enmPageLoadEvent);
	};
	function RunQueue(enmPageLoadEvent) {
		// run all functions in the queue for the given page load event
		for (var i = 0; i < arrQueue.length; ++i) {
			var objFunction = arrQueue[i];
			if (objFunction.Type === enmPageLoadEvent) {
				objFunction.Function.apply(null, objFunction.Parameters);
			}
		}
	};

	// public functions
	objController.Init = function (dicJavaScriptVariables, dicJavaScriptFunctions) {
		Initialize(dicJavaScriptVariables, dicJavaScriptFunctions, PageLoadEvent.Init);
	};
	objController.InitReady = function () {
		Initialize(ParseCodeBehindJson(GetEmbeddedJson(VARCONTAINERID)), ParseCodeBehindJson(GetEmbeddedJson(FUNCCONTAINERID)), PageLoadEvent.Ready);
	};

	return objController;
}(window, document));
/// <reference path="json2.min.js"/>

var JSMController = (function (window, document) {
	'use strict';
	var objController = {};
	// private properties
	var VARCONTAINERID = "JSMVariables";
	var FUNCCONTAINERID = "JSMFunctions";
	var PageLoadEvent = { Init: 0, Ready: 1 };
	var arrQueue = [];

	// private functions
	function GetEmbeddedJson(strId) {
		var strJson = document.getElementById(strId).innerHTML;
		var bolComment = (strJson.indexOf("<") === 0) || (strJson.indexOf("&lt;") === 0); //check if we are in another browser than IE
		if (!bolComment) // IE detected, return Json
			return strJson.replace(/\&amp;/g, "&"); //ie is messing with some templates
		var bolLtEntity = (strJson.indexOf("&lt;") === 0); // Fix for Firefox 3.6
		return strJson.substring(bolLtEntity ? 20 : 14, (strJson.length - (bolLtEntity ? 18 : 12))); //return substring without html comment tags
	};
	function ParseCodeBehindJson(strJson) {
		return strJson ? JSON.parse(strJson) : null;
	};
	function FillCodeBehindJsVariables(dictVariables) {
		if (!dictVariables)
			return;
		for (var strVariableName in dictVariables) {
			var objValue = dictVariables[strVariableName];
			var arrVariableName = strVariableName.split(".");
			var objCurrent = window;
			// If the variable contains '.' we have to get or create the literals iteratively
			for (var i = 0; i < arrVariableName.length - 1; ++i) {
				//literal not found --> create new one
				if (!objCurrent[arrVariableName[i]]) {
					objCurrent[arrVariableName[i]] = {};
				}
				objCurrent = objCurrent[arrVariableName[i]];
			}
			// finally set the value
			objCurrent[arrVariableName[arrVariableName.length - 1]] = objValue;
		}
	};
	function FillCodeBehindJavaScriptFunctions(dictFunctions, enmPageLoadEvent) {
		if (!dictFunctions)
			return;
		for (var i = 0; i < dictFunctions.length; ++i) {
			var objFunctionContainer = dictFunctions[i];
			var arrFunctionName = objFunctionContainer.Function.split(".");
			var delFunction = window;
			// If the function name contains '.' we have to get the function iteratively
			for (var j = 0; j < arrFunctionName.length; ++j) {
				delFunction = delFunction[arrFunctionName[j]];
			}
			if (typeof delFunction !== "function")
				throw new Error(objFunctionContainer.Function + " is not a valid function.");
			// store the function reference in the queue
			arrQueue.push({ Type: enmPageLoadEvent, Function: delFunction, Parameters: objFunctionContainer.Parameters });
		}
	};
	function Initialize(dicJavaScriptVariables, dicJavaScriptFunctions, enmPageLoadEvent) {
		FillCodeBehindJsVariables(dicJavaScriptVariables);
		FillCodeBehindJavaScriptFunctions(dicJavaScriptFunctions, enmPageLoadEvent);
		RunQueue(enmPageLoadEvent);
	};
	function RunQueue(enmPageLoadEvent) {
		// run all functions in the queue for the given page load event
		for (var i = 0; i < arrQueue.length; ++i) {
			var objFunction = arrQueue[i];
			if (objFunction.Type === enmPageLoadEvent) {
				objFunction.Function.apply(null, objFunction.Parameters);
			}
		}
	};

	// public functions
	objController.Init = function (dicJavaScriptVariables, dicJavaScriptFunctions) {
		Initialize(dicJavaScriptVariables, dicJavaScriptFunctions, PageLoadEvent.Init);
	};
	objController.InitReady = function () {
		Initialize(ParseCodeBehindJson(GetEmbeddedJson(VARCONTAINERID)), ParseCodeBehindJson(GetEmbeddedJson(FUNCCONTAINERID)), PageLoadEvent.Ready);
	};

	return objController;
}(window, document));