import { useState, useEffect } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { Label } from "@/components/ui/label";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { ChevronDown, ChevronUp, MoreHorizontal, Loader2 } from "lucide-react";
import useAuth from "@/hooks/useAuth";
import { useNavigate } from "react-router-dom";
import { PATH_PUBLIC } from "@/router/path";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { getCustomerById, updateCustomerInfo } from "@/apis/auth.services";
import { toast } from "react-toastify";
import { ICustomerResponse } from "@/types/customer.type";

// interface ICustomerResponse {
//   customerId: string;
//   userId: string;
//   fullName: string;
//   gender: string;
//   email: string;
//   phoneNumber: string;
//   birthDate: string;
//   avatarUrl: string;
//   country: string;
//   address: string;
// }

interface IUpdateCustomer {
  customerId: string;
  fullName: string;
  gender: string;
  birthDate: string;
  country: string;
  address: string;
}

type SortKey = "fullName" | "email" | "phoneNumber";

export default function Users() {
  const [users, setUsers] = useState<ICustomerResponse[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState("");
  const [sortKey, setSortKey] = useState<SortKey>("fullName");
  const [sortOrder, setSortOrder] = useState<"asc" | "desc">("asc");
  const [currentPage, setCurrentPage] = useState(1);
  const [selectedUser, setSelectedUser] = useState<string | null>(null);
  const [isViewDialogOpen, setIsViewDialogOpen] = useState(false);
  const [isEditDialogOpen, setIsEditDialogOpen] = useState(false);
  const itemsPerPage = 10;

  const navigate = useNavigate();
  const queryClient = useQueryClient();

  const { isAuthenticated, getCustomerAll } = useAuth();

  const { data: selectedUserData, refetch: refetchSelectedUser } = useQuery({
    queryKey: ["customerById", selectedUser],
    queryFn: () => getCustomerById(selectedUser as string),
    enabled: !!selectedUser,
  });

  useEffect(() => {
    if (!isAuthenticated) {
      navigate(PATH_PUBLIC.home);
    }

    const fetchAllCustomers = async () => {
      const customers: ICustomerResponse[] | null | undefined =
        await getCustomerAll(); // Lấy tất cả khách hàng
      if (customers) {
        setUsers(customers);
      }
      setLoading(false);
    };

    fetchAllCustomers();
  }, [isAuthenticated, getCustomerAll, navigate]);

  const updateCustomerMutation = useMutation({
    mutationFn: (data: IUpdateCustomer) => updateCustomerInfo(data),
    onSuccess: (data) => {
      if (data.isSuccess) {
        console.log("Data: ", data);

        toast.success(data.message);
        if (selectedUser) {
          // Chỉ cần truyền một mảng có thể là một string
          queryClient.invalidateQueries({
            queryKey: ["customerById", selectedUser],
          });
        }
        setIsEditDialogOpen(false);
      } else {
        toast.error(data.message);
      }
    },
    onError: (error) => {
      console.log("Error when update user information", error);
      toast.error("Failed to update user information");
    },
  });

  const filteredUsers = users.filter(
    (user) =>
      user.fullName.toLowerCase().includes(searchTerm.toLowerCase()) ||
      user.email.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const sortedUsers = [...filteredUsers].sort((a, b) => {
    if (a[sortKey] < b[sortKey]) return sortOrder === "asc" ? -1 : 1;
    if (a[sortKey] > b[sortKey]) return sortOrder === "asc" ? 1 : -1;
    return 0;
  });

  const paginatedUsers = sortedUsers.slice(
    (currentPage - 1) * itemsPerPage,
    currentPage * itemsPerPage
  );

  const totalPages = Math.ceil(sortedUsers.length / itemsPerPage);

  const handleSort = (key: SortKey) => {
    if (key === sortKey) {
      setSortOrder(sortOrder === "asc" ? "desc" : "asc");
    } else {
      setSortKey(key);
      setSortOrder("asc");
    }
  };

  const handleViewUser = (customerId: string) => {
    setSelectedUser(customerId);
    refetchSelectedUser();
    setIsViewDialogOpen(true);
  };

  const handleEditUser = (customerId: string) => {
    setSelectedUser(customerId);
    refetchSelectedUser();
    setIsEditDialogOpen(true);
  };

  const handleUpdateUser = (updatedData: IUpdateCustomer) => {
    if (selectedUser) {
      updateCustomerMutation.mutate({
        // customerId: selectedUser,
        ...updatedData,
      });
    }
  };

  return (
    <div className='p-6 bg-gray-50'>
      <div className='flex justify-between items-center mb-6'>
        <h1 className='text-3xl font-bold text-gray-800'>User Management</h1>
      </div>
      <div className='mb-4 flex justify-between items-center'>
        <Input
          type='text'
          placeholder='Search users...'
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          className='max-w-sm'
        />
        <div className='text-sm text-gray-500'>
          Showing {paginatedUsers.length} of {sortedUsers.length} users
        </div>
      </div>
      {loading ? (
        <div className='flex justify-center items-center h-64'>
          <Loader2 className='h-8 w-8 animate-spin text-primary' />
        </div>
      ) : (
        <div className='bg-white rounded-lg shadow overflow-hidden'>
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead className='w-[200px]'>
                  <button
                    className='flex items-center text-left font-semibold text-gray-900'
                    onClick={() => handleSort("fullName")}>
                    Full Name
                    {sortKey === "fullName" &&
                      (sortOrder === "asc" ? (
                        <ChevronUp className='ml-2 h-4 w-4' />
                      ) : (
                        <ChevronDown className='ml-2 h-4 w-4' />
                      ))}
                  </button>
                </TableHead>
                <TableHead>
                  <button
                    className='flex items-center text-left font-semibold text-gray-900'
                    onClick={() => handleSort("email")}>
                    Email
                    {sortKey === "email" &&
                      (sortOrder === "asc" ? (
                        <ChevronUp className='ml-2 h-4 w-4' />
                      ) : (
                        <ChevronDown className='ml-2 h-4 w-4' />
                      ))}
                  </button>
                </TableHead>
                <TableHead>
                  <button
                    className='flex items-center text-left font-semibold text-gray-900'
                    onClick={() => handleSort("phoneNumber")}>
                    Phone Number
                    {sortKey === "phoneNumber" &&
                      (sortOrder === "asc" ? (
                        <ChevronUp className='ml-2 h-4 w-4' />
                      ) : (
                        <ChevronDown className='ml-2 h-4 w-4' />
                      ))}
                  </button>
                </TableHead>
                <TableHead className='text-right'>Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {paginatedUsers.map((user) => (
                <TableRow key={user.customerId} className='hover:bg-gray-50'>
                  <TableCell className='font-medium'>{user.fullName}</TableCell>
                  <TableCell>{user.email}</TableCell>
                  <TableCell>{user.phoneNumber}</TableCell>
                  <TableCell className='text-right'>
                    <DropdownMenu>
                      <DropdownMenuTrigger asChild>
                        <Button variant='ghost' className='h-8 w-8 p-0'>
                          <span className='sr-only'>Open menu</span>
                          <MoreHorizontal className='h-4 w-4' />
                        </Button>
                      </DropdownMenuTrigger>
                      <DropdownMenuContent align='end'>
                        <DropdownMenuLabel>Actions</DropdownMenuLabel>
                        <DropdownMenuItem
                          onClick={() => handleViewUser(user.customerId)}>
                          View user
                        </DropdownMenuItem>
                        <DropdownMenuItem
                          onClick={() => handleEditUser(user.customerId)}>
                          Edit user
                        </DropdownMenuItem>
                      </DropdownMenuContent>
                    </DropdownMenu>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </div>
      )}
      <div className='flex items-center justify-end space-x-2 py-4'>
        <Button
          variant='outline'
          size='sm'
          onClick={() => setCurrentPage((prev) => Math.max(prev - 1, 1))}
          disabled={currentPage === 1}>
          Previous
        </Button>
        <Button
          variant='outline'
          size='sm'
          onClick={() =>
            setCurrentPage((prev) => Math.min(prev + 1, totalPages))
          }
          disabled={currentPage === totalPages}>
          Next
        </Button>
      </div>

      {/* View User Dialog */}
      <Dialog open={isViewDialogOpen} onOpenChange={setIsViewDialogOpen}>
        <DialogContent className='sm:max-w-[425px]'>
          <DialogHeader>
            <DialogTitle>User Information</DialogTitle>
          </DialogHeader>
          {selectedUserData && selectedUserData.result && (
            <div className='grid gap-4 py-4'>
              <div className='grid grid-cols-4 items-center gap-4'>
                <Label className='text-right font-medium'>Full Name</Label>
                <div className='col-span-3'>
                  {selectedUserData.result.fullName}
                </div>
              </div>
              <div className='grid grid-cols-4 items-center gap-4'>
                <Label className='text-right font-medium'>Email</Label>
                <div className='col-span-3'>
                  {selectedUserData.result.email}
                </div>
              </div>
              <div className='grid grid-cols-4 items-center gap-4'>
                <Label className='text-right font-medium'>Phone Number</Label>
                <div className='col-span-3'>
                  {selectedUserData.result.phoneNumber}
                </div>
              </div>
              <div className='grid grid-cols-4 items-center gap-4'>
                <Label className='text-right font-medium'>Gender</Label>
                <div className='col-span-3'>
                  {selectedUserData.result.gender}
                </div>
              </div>
              <div className='grid grid-cols-4 items-center gap-4'>
                <Label className='text-right font-medium'>Birth Date</Label>
                <div className='col-span-3'>
                  {new Date(
                    selectedUserData.result.birthDate
                  ).toLocaleDateString()}
                </div>
              </div>
              <div className='grid grid-cols-4 items-center gap-4'>
                <Label className='text-right font-medium'>Country</Label>
                <div className='col-span-3'>
                  {selectedUserData.result.country}
                </div>
              </div>
              <div className='grid grid-cols-4 items-center gap-4'>
                <Label className='text-right font-medium'>Address</Label>
                <div className='col-span-3'>
                  {selectedUserData.result.address}
                </div>
              </div>
            </div>
          )}
        </DialogContent>
      </Dialog>

      {/* Edit User Dialog */}
      <Dialog open={isEditDialogOpen} onOpenChange={setIsEditDialogOpen}>
        <DialogContent className='sm:max-w-[425px]'>
          <DialogHeader>
            <DialogTitle>Edit User</DialogTitle>
          </DialogHeader>
          {selectedUserData && selectedUserData.result && (
            <form
              onSubmit={(e) => {
                e.preventDefault();
                const formData = new FormData(e.currentTarget);
                const updatedData: IUpdateCustomer = {
                  customerId: selectedUser as string,
                  fullName: "",
                  gender: "",
                  birthDate: "",
                  country: "",
                  address: "",
                };

                const fields = [
                  "fullName",
                  "gender",
                  "birthDate",
                  "country",
                  "address",
                ] as const;
                fields.forEach((field) => {
                  const value = formData.get(field);
                  if (
                    value !== null
                    // value !== selectedUserData.result[field]
                  ) {
                    updatedData[field] = value as string;
                  }
                });

                if (Object.keys(updatedData).length > 1) {
                  handleUpdateUser(updatedData);
                } else {
                  toast.info("No changes detected");
                }
              }}>
              <div className='grid gap-4 py-4'>
                <div className='grid grid-cols-4 items-center gap-4'>
                  <Label htmlFor='fullName' className='text-right'>
                    Full Name
                  </Label>
                  <Input
                    id='fullName'
                    name='fullName'
                    defaultValue={selectedUserData.result.fullName}
                    className='col-span-3'
                  />
                </div>
                <div className='grid grid-cols-4 items-center gap-4'>
                  <Label htmlFor='gender' className='text-right'>
                    Gender
                  </Label>
                  <Select
                    name='gender'
                    defaultValue={selectedUserData.result.gender}>
                    <SelectTrigger className='col-span-3'>
                      <SelectValue placeholder='Select gender' />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value='male'>Male</SelectItem>
                      <SelectItem value='female'>Female</SelectItem>
                      <SelectItem value='other'>Other</SelectItem>
                    </SelectContent>
                  </Select>
                </div>
                <div className='grid grid-cols-4 items-center gap-4'>
                  <Label htmlFor='birthDate' className='text-right'>
                    Birth Date
                  </Label>
                  <Input
                    id='birthDate'
                    name='birthDate'
                    type='date'
                    defaultValue={
                      selectedUserData.result.birthDate.split("T")[0]
                    }
                    className='col-span-3'
                  />
                </div>
                <div className='grid grid-cols-4 items-center gap-4'>
                  <Label htmlFor='country' className='text-right'>
                    Country
                  </Label>
                  <Input
                    id='country'
                    name='country'
                    defaultValue={selectedUserData.result.country}
                    className='col-span-3'
                  />
                </div>
                <div className='grid grid-cols-4 items-center gap-4'>
                  <Label htmlFor='address' className='text-right'>
                    Address
                  </Label>
                  <Input
                    id='address'
                    name='address'
                    defaultValue={selectedUserData.result.address}
                    className='col-span-3'
                  />
                </div>
              </div>
              <div className='flex justify-end mt-4'>
                <Button type='submit'>Update User</Button>
              </div>
            </form>
          )}
        </DialogContent>
      </Dialog>
    </div>
  );
}
