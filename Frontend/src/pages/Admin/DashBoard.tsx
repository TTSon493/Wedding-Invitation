import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from "../../components/ui/card";
import { Progress } from "../../components/ui/progress";

const DashBoard = () => {
  return (
    <div className='p-6'>
      <h1 className='text-3xl font-bold mb-6'>Wedding Dashboard</h1>
      <div className='grid gap-6 md:grid-cols-2 lg:grid-cols-4'>
        <Card>
          <CardHeader className='flex flex-row items-center justify-between space-y-0 pb-2'>
            <CardTitle className='text-sm font-medium'>Total Guests</CardTitle>
          </CardHeader>
          <CardContent>
            <div className='text-2xl font-bold'>250</div>
            <Progress value={75} className='mt-2' />
            <p className='text-xs text-muted-foreground mt-2'>75% responded</p>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className='flex flex-row items-center justify-between space-y-0 pb-2'>
            <CardTitle className='text-sm font-medium'>Budget</CardTitle>
          </CardHeader>
          <CardContent>
            <div className='text-2xl font-bold'>$15,000 / $20,000</div>
            <Progress value={75} className='mt-2' />
            <p className='text-xs text-muted-foreground mt-2'>75% allocated</p>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className='flex flex-row items-center justify-between space-y-0 pb-2'>
            <CardTitle className='text-sm font-medium'>
              Tasks Completed
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className='text-2xl font-bold'>24 / 50</div>
            <Progress value={48} className='mt-2' />
            <p className='text-xs text-muted-foreground mt-2'>48% completed</p>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className='flex flex-row items-center justify-between space-y-0 pb-2'>
            <CardTitle className='text-sm font-medium'>
              Days Until Wedding
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className='text-2xl font-bold'>45</div>
            <p className='text-xs text-muted-foreground mt-2'>
              Save the date: July 15, 2024
            </p>
          </CardContent>
        </Card>
      </div>
      <div className='mt-6'>
        <Card>
          <CardHeader>
            <CardTitle>Invitation Responses Over Time</CardTitle>
          </CardHeader>
        </Card>
      </div>
    </div>
  );
};

export default DashBoard;
