import React from "react";
import { useAuth } from "../context/AuthContext";

interface SidenavProps {
  appState: number;
  setAppState: React.Dispatch<React.SetStateAction<number>>;
}

const Sidenav: React.FC<SidenavProps> = ({ appState, setAppState }) => {
  const auth = useAuth();
  return (
    <div className="flex flex-col bg-blue-600 text-white justify-between w-[10%]">
      <div className="flex flex-col">
        <h2 className="text-lg font-bold text-center pt-4 pb-8">Dashboard</h2>
        <button
          onClick={() => setAppState(0)}
          className={`p-4 hover:bg-blue-700 ${
            appState == 0 ? "bg-blue-700" : ""
          }`}
        >
          Menu
        </button>
        <button
          onClick={() => setAppState(1)}
          className={`p-4 hover:bg-blue-700 ${
            appState == 1 ? "bg-blue-700" : ""
          }`}
        >
          Orders
        </button>
        <button
          onClick={() => setAppState(2)}
          className={`p-4 hover:bg-blue-700 ${
            appState == 2 ? "bg-blue-700" : ""
          }`}
        >
          Create Order
        </button>
        <button
          onClick={() => setAppState(3)}
          className={`p-4 hover:bg-blue-700 ${
            appState == 3 ? "bg-blue-700" : ""
          }`}
        >
          New Food
        </button>
      </div>
      <button onClick={auth.logout} className="p-4 hover:bg-blue-700">
        Logout
      </button>
    </div>
  );
};

export default Sidenav;
