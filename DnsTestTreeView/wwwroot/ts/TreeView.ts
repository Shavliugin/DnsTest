///<reference path="node_modules/@types/jquery/dist/jquery.slim.d.ts" />
const treeNodeId = "";
const nodesUrl = "https://localhost:44363/api/treeNodes/";

enum PlaceType {
    Inside = 0,
    Above = 1,
    Below = 2
}

class TreeNode {
    id: number;
    parentId: number;
    name: string;
    isDirectory: boolean;    

    constructor(id: number, parentId: number, name: string, isDirectory: boolean) {
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

async function drop(event) {

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
        
        const list = dropzone.parentElement.querySelector(".nested") as HTMLElement;
        
        containerId = list.parentElement.id;

            if (!list.classList.contains("listIsLoaded")) {
            list.classList.add("listIsLoaded");
            await createAndGetNodeChildList(list, parseInt(list.parentElement.id));
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
    
}

function createAndGetNodeChildList(container: HTMLElement, nodeId: number | null) {    
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

function setDraggable(item: HTMLElement) {
    item.draggable = true;
    
    item.ondragstart = function (event) {
        drag(event)
    };
    item.ondrop = async function (event) {
        drop(event)
    };
    item.ondragover = function (event) {
        allowDrop(event);
    };
}

function createTreeByTreeNode(container: HTMLElement, nodes: TreeNode[]) {
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

async function replaceTreeNode(draggableTreeNodeId: number, replaceTreeNodeId: number) {
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
}

function startWaitingAnimation() {
    document.getElementById("loadingAnimation").classList.remove("nested");
}

function stopWaitingAnimation() {
    document.getElementById("loadingAnimation").classList.add("nested");
}

createAndGetNodeChildList(document.getElementById("Root"), null);