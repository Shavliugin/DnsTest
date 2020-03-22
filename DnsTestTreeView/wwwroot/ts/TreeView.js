var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
///<reference path="node_modules/@types/jquery/dist/jquery.slim.d.ts" />
const treeNodeId = "";
const nodesUrl = "https://localhost:44363/api/treeNodes/";
var PlaceType;
(function (PlaceType) {
    PlaceType[PlaceType["Inside"] = 0] = "Inside";
    PlaceType[PlaceType["Above"] = 1] = "Above";
    PlaceType[PlaceType["Below"] = 2] = "Below";
})(PlaceType || (PlaceType = {}));
class TreeNode {
    constructor(id, parentId, name, isDirectory) {
        this.id = id;
        this.parentId = parentId;
        this.name = name;
        this.isDirectory = isDirectory;
    }
}
function allowDrop(event) {
    event.preventDefault();
}
function drag(event) {
    event
        .dataTransfer
        .setData("text/plain", event.target.id);
}
function drop(event) {
    return __awaiter(this, void 0, void 0, function* () {
        //Так и не понял, баг это или мои кривые руки, но drop вызывался два раза с одними и теми же параметрами. Пришлось выкручиваться ¯\_(ツ)_/¯
        if (!$('.already-dropped').length) {
            $('body').addClass('already-dropped');
            setTimeout(function () {
                $('.already-dropped').removeClass('already-dropped');
            }, 100);
        }
        else {
            return;
        }
        const id = event
            .dataTransfer
            .getData("text");
        const draggableElement = document.getElementById(id);
        let dropzone = event.target;
        let containerId = dropzone.id;
        if (dropzone.classList.contains("caret")) {
            const list = dropzone.parentElement.querySelector(".nested");
            containerId = list.parentElement.id;
            if (!list.classList.contains("listIsLoaded")) {
                list.classList.add("listIsLoaded");
                yield createAndGetNodeChildList(list, parseInt(list.parentElement.id));
                dropzone.classList.add("caret-down");
                list.classList.add("active");
            }
            replaceTreeNode(parseInt(id), parseInt(containerId));
            list.appendChild(draggableElement);
        }
        else {
            replaceTreeNode(parseInt(id), parseInt(containerId));
            dropzone.appendChild(draggableElement);
        }
    });
}
function createAndGetNodeChildList(container, nodeId) {
    return $.ajax({
        url: nodesUrl + (nodeId == null ? "" : nodeId.toString()),
        async: false,
        dataType: "json",
        success: c => {
            createTreeByTreeNode(container, c);
        },
        error: (e, errorMessage) => {
            console.log(e.statusCode);
            console.log(errorMessage);
        }
    });
}
function setDraggable(item) {
    item.draggable = true;
    item.ondragstart = function (event) {
        drag(event);
    };
    item.ondrop = function (event) {
        return __awaiter(this, void 0, void 0, function* () {
            drop(event);
        });
    };
    item.ondragover = function (event) {
        allowDrop(event);
    };
}
function createTreeByTreeNode(container, nodes) {
    for (let node of nodes) {
        let li = document.createElement("li");
        li.id = treeNodeId + node.id.toString();
        setDraggable(li);
        if (node.isDirectory) {
            let ul = document.createElement("ul");
            ul.classList.add("nested");
            let span = document.createElement("span");
            span.classList.add("caret");
            span.innerHTML = node.name;
            span.addEventListener("click", function () {
                if (!ul.classList.contains("listIsLoaded")) {
                    ul.classList.add("listIsLoaded");
                    createAndGetNodeChildList(ul, node.id);
                }
                this.parentElement.querySelector(".nested").classList.toggle("active");
                this.classList.toggle("caret-down");
            });
            li.append(span);
            li.append(ul);
        }
        else {
            li.innerHTML = node.name;
        }
        container.append(li);
        container.classList.add("listIsLoaded");
    }
}
function replaceTreeNode(draggableTreeNodeId, replaceTreeNodeId) {
    return __awaiter(this, void 0, void 0, function* () {
        startWaitingAnimation();
        return $.ajax({
            url: nodesUrl + draggableTreeNodeId.toString() + "/" + replaceTreeNodeId.toString() + "/",
            async: true,
            success: () => {
                stopWaitingAnimation();
                console.log("save success!");
            },
            error: (e) => {
                console.log(e);
            }
        });
    });
}
function startWaitingAnimation() {
    document.getElementById("loadingAnimation").classList.remove("nested");
}
function stopWaitingAnimation() {
    document.getElementById("loadingAnimation").classList.add("nested");
}
createAndGetNodeChildList(document.getElementById("Root"), null);
//# sourceMappingURL=TreeView.js.map