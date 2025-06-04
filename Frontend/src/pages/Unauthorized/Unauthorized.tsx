import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { Button } from "@/components/ui/button";
import useAuth from "@/hooks/useAuth";
import { PATH_ADMIN, PATH_PUBLIC } from "@/router/path";
import { RolesEnum } from "@/types/auth.type";
import { Terminal } from "lucide-react";
import { useNavigate } from "react-router-dom";

const Unauthorized = () => {
  const navigate = useNavigate();
  const { user } = useAuth();
  const role = user?.roles[0];
  return (
    <>
      <Alert>
        <Terminal className='h-4 w-4' />
        <AlertTitle>Heads up!</AlertTitle>
        <AlertDescription>
          Sorry, you are not authorized to access this page.
          <Button
            onClick={() =>
              navigate(
                role === RolesEnum.ADMIN
                  ? PATH_ADMIN.dashboard
                  : PATH_PUBLIC.home
              )
            }>
            Back home
          </Button>
        </AlertDescription>
      </Alert>
    </>
  );
};

export default Unauthorized;
