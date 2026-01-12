import Header from "./Header";
import Footer from "./Footer";
import Foot from "./Foot";
import Card from "./Card";
import Student from "./Student";
import UserGreeting from "./UserGreeting";
import List from "./List";
import Button from "./Button";
import MyComponent from "./MyComponent";
import Books from "./Books";
import ComponentA from "./ComponentA";
import HomePage from "./HomePage";
import LoginPage from "./LoginPage";

import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { Router } from "react-router-dom";

const queryClient = new QueryClient();

function App() {
  return (
    <>
      <QueryClientProvider client={queryClient}>
        {/* <Header />
        <Foot />
        <Student name="Hoàng" age={20} isStudent={true} />
        <UserGreeting isLoggedIn={false} username="hoaug" />
        <Button />
        <List />
        <Card />
        <Footer />
        <MyComponent />
        <Books />
        <ComponentA /> */}
        <BrowserRouter>
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/login" element={<LoginPage />} />
          </Routes>
        </BrowserRouter>
      </QueryClientProvider>
    </>
  );
}

export default App;
