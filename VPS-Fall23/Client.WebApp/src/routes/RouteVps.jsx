import { Route } from 'react-router-dom';
import { useEffect } from 'react';
const RouteVps = (index,layout,route) => {
  const Layout = layout;
  const Page = route.component;
  let subRoutes = route.subRoutes;

  useEffect(() => {

  }, []);

  return (
    <>
      <Route
        key={index}
        path={route.path}
        element={
          <Layout>
            <Page></Page>
          </Layout>
        }
      >
        {subRoutes !== undefined &&
          subRoutes.map((subRoute, index) => {
            return (
              <Route
                key={index}
                path={subRoute.url}
                element={<subRoute.component></subRoute.component>}
              ></Route>
            );
          })}
      </Route>
    </>
  );
}

export default RouteVps;