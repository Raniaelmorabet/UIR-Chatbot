import React, { useState, useEffect } from "react";
import "./Sidebar.css";
import { assets } from "../../assets/assets.js";
import { RiMenuFold2Line, RiMenuUnfold2Line } from "react-icons/ri";
import { MdDeleteOutline } from "react-icons/md";
import { MdHistory } from "react-icons/md";
import { useLocation, useNavigate } from "react-router-dom";

const Sidebar = () => {
    const port = `http://localhost:5182/api/Conversations`;
    const [extended, setExtended] = useState(false);
    const [conversations, setConversations] = useState([]);

    //new--

    const navigate = useNavigate();
    const location = useLocation();
    const userId = location.state?.userId;

    //new
    useEffect(() => {
        if (!userId) {
            navigate("/");
        } else {
            fetch(`http://localhost:5182/Conversation/IdUser/${userId}`)
                .then((response) => response.json())
                .then((data) => {
                    setConversations(data["$values"]);
                })
                .catch((error) => {
                    console.error("Error fetching data:", error);
                });
        }
    }, [userId, navigate]);

    //new
    const handleLogout = () => {
        navigate("/");
    };

    //front
    const truncateText = (text) => {
        if (!text) return "";
        return text.split(" ").slice(0, 5).join(" ");
    };

    // const deleteConversation = async (id) => {
    //     if (window.confirm('Are you sure you want to delete this conversation?')) {
    //         try {
    //             const response = await fetch(`${port}/${id}`, {
    //                 method: 'DELETE',
    //             });
    //             if (response.ok) {
    //                 fetchConversations();
    //             } else {
    //                 console.error('Failed to delete conversation:', response.statusText);
    //             }
    //         } catch (error) {
    //             console.error('Error deleting conversation:', error);
    //         }
    //     }
    // };

    const handleNewChat = () => {
        window.location.reload();
    };

    return (
        <div className={`sidebar `}>
            <div className="top">
                <button
                    onClick={() => setExtended((prev) => !prev)}
                    className={`menu ${
                        extended
                            ? "transform scale-x-[-1] transition-transform duration-2000 ease-in-out "
                            : "transition-transform duration-2000 ease-in-out"
                    }`}
                >
                    <RiMenuFold2Line />
                </button>
                <div className="new-chat" onClick={handleNewChat}>
                    <img className="plus-icon" src={assets.plus_icon} alt="plusIcon" />
                    {extended ? <p>New Chat</p> : null}
                </div>
                {extended ? (
                    <div className="recent">
                        <p className="recent-title">Recent history</p>
                        <div className="recent-entry">
                            <ul>
                                <div id="data-container">
                                    {conversations.map((conversation) =>
                                        conversation.questions["$values"].map((question) => (
                                            <div key={question.questionContent} className='text-white/80 flex '>
                                                <h2>{truncateText(question.questionContent)}</h2>
                                            </div>
                                        ))
                                    )}
                                </div>
                            </ul>
                        </div>
                    </div>
                ) : null}
            </div>
            <img src={assets.uir_icon} className="sidebar-image" />
        </div>
    );
};

export default Sidebar;