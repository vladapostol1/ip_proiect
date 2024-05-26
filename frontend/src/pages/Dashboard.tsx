import React, { useState } from "react";
import Sidenav from "../components/Sidenav";
import Menu from "../components/Menu";
import Orders from "../components/Orders";
import MakeOrder from "../components/MakeOrder";
import NewFood from "../components/NewFood";
import Help from "../components/Help";

const Dashboard: React.FC = () => {
  const [appState, setAppState] = useState(0);

  const goToHelp = () => {
    setAppState(4)
  }

  return (
    <div className="flex flex-row min-h-screen bg-zinc-950">
      <Sidenav appState={appState} setAppState={setAppState} />
      <div className="flex-grow">
        {appState == 0 ? (
          <Menu />
        ) : appState == 1 ? (
          <Orders />
        ) : appState == 2 ? (
          <MakeOrder />
        ) : appState == 3 ? (
          <NewFood />
        ) : appState == 4 ? (
          <Help />
        ) : (
          <div>An error has appeard.</div>
        )}
      </div>
      {appState !== 4 && (
  <button 
    className="absolute bottom-4 right-4 rounded-full px-4 py-2 bg-blue-600 hover:bg-blue-700 border-2 border-white" 
    onClick={goToHelp}
  >
    ?
  </button>
)}
    </div>
  );
};

export default Dashboard;
