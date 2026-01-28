import HomePage from "./page/homepage/HomePage";
import LoginPage from "./page/authpage/LoginPage";
import RegisterPage from "./page/authpage/RegisterPage";
import NotificationProvider from "./page/common/NotificationContext";
import UserPage from "./page/homepage/UserPage";

import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { BrowserRouter, Routes, Route } from "react-router-dom";

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
        <NotificationProvider
          child={
            <BrowserRouter>
              <Routes>
                <Route path="/" element={<HomePage />} />
                <Route path="/login" element={<LoginPage />} />
                <Route path="/register" element={<RegisterPage />} />
                <Route path="/admin/users" element={<UserPage />} />
              </Routes>
            </BrowserRouter>
          }
        />
      </QueryClientProvider>
    </>
  );
}

export default App;
