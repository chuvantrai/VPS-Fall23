import { Route, Routes } from "react-router-dom";
import routes from "../routes";


const RouteWithNoLayout = () => {
    return <Routes>
        {
            routes.noLayout.map((route, index) => {
                return <Route key={index} path={route.path} Component={route.component} />
            })
        }
    </Routes>
}
export default RouteWithNoLayout;