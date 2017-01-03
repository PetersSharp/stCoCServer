
window.MsgPrn = function(id, user, val)
{
    var cdate = new Date();
    var eleroot;
    var eleclass = "";
    switch(id)
    {
        case "msguser":
        case "msgmy":
        case "msgpriv": {
            eleroot = document.createElement("div");
            break;
        }
        case "msghistory":
        case "msgserver":
        case "msgnotice": {
            eleroot = document.createElement("p");
            break;
        }
        default: {
            return;
        }
    }
    switch(id)
    {
        case "msguser": {
            eleclass = "speech msguser left";
            break;
        }
        case "msgmy": {
            eleclass = "speech msgmy right";
            break;
        }
        case "msgpriv": {
            eleclass = "speech msgpriv right";
            break;
        }
        case "msghistory": {
            eleclass = "msghistory right";
            break;
        }
        case "msgnotice": {
            eleclass = "msgsys msgnotice";
            break;
        }
        case "msgserver": {
            eleclass = "msgsys msgserver";
            break;
        }
        default: {
            return;
        }
    }
    switch(id)
    {
        case "msguser":
        case "msgmy":
        case "msgpriv": {
            var elehead = document.createElement("div");
            var eletime = document.createElement("span");
            var elehref = document.createElement("a");
            elehead.setAttribute("class", "uid");
            elehref.setAttribute("href", "javascript:void(0);");
            elehref.setAttribute("onclick", "javascript:getChatMenu('" + user + "');");
            eletime.setAttribute("class", "ptime");
            elehref.innerHTML = user;
            eletime.innerHTML = cdate.toLocaleTimeString();
            elehead.appendChild(elehref);
            elehead.appendChild(eletime);
            eleroot.appendChild(elehead);
            eleroot.innerHTML += val;
            break;
        }
        case "msghistory": {
            var ln = (((user.length - 7) > 20) ? 20 : user.length);
            var elehref = document.createElement("a");
            elehref.setAttribute("href", user);
            elehref.innerHTML = val + " " + user.substring(7, ln) + "..";
            eleroot.appendChild(elehref);
            break;
        }
        case "msgnotice": {
            eleroot.innerHTML = cdate.toLocaleTimeString() +  " - " +  user + " -&gt; " + val;
            break;
        }
        case "msgserver": {
            eleroot.innerHTML = val;
            break;
        }
    }
    eleroot.setAttribute("class", eleclass);
    document.body.appendChild(eleroot);
    window.scrollTo(0, document.documentElement.scrollHeight);
}
window.DisableSelection = function()
{
    document.body.onselectstart=function(){ return false; }; 
    document.body.ondragstart=function() { return false; };
}
window.getChatMenu = function(val)
{
    window.external.WBChat_GetChatMenu(val);
    return false;
}
